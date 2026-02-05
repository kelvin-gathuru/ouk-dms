
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.LuceneHandler;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using J2N.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class AddDocumentIndexContentCommandHandler(IDocumentIndexRepository _documentIndexRepository,
     IDocumentRepository _documentRepository,
     IDocumentVersionRepository documentVersionRepository,
     StorageServiceFactory _storeageServiceFactory,
     IStorageSettingRepository _storageSettingRepository,
     IWebHostEnvironment _webHostEnvironment,
     IDocumentChunkRepository documentChunkRepository,
      IUnitOfWork<DocumentContext> _uow,
      ILogger<AddDocumentIndexContentCommandHandler> _logger,
      PathHelper pathHelper,
     IHttpClientFactory httpClientFactory
    ) : IRequestHandler<AddDocumentIndexContentCommand, bool>
{
    public async Task<bool> Handle(AddDocumentIndexContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var documentIndex = await _documentIndexRepository.All.OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
            if (documentIndex == null)
            {
                return false;
            }
            var documentVersion = await documentVersionRepository.All.FirstOrDefaultAsync(c => c.Id == documentIndex.DocumentVersionId);
            if (documentVersion == null && !documentVersion.IsAllChunkUploaded)
            {
                return false;
            }

            var document = await _documentRepository.All.FirstOrDefaultAsync(c => c.Id == documentVersion.DocumentId);
            if (document != null)
            {
                var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);
                if (storeageSetting == null)
                {
                    return false;
                }
                var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
                byte[] responseBytes;
                if (documentVersion.IsChunk)
                {
                    responseBytes = await CombineChunkBytes(documentVersion, document, storeageSetting, storageService);
                }
                else
                {
                    var fileResult = await storageService.DownloadFileAsync(documentVersion.Url, storeageSetting.JsonValue, document.Key, document.IV);
                    responseBytes = fileResult.FileBytes;
                }
                try
                {
                    string extension = Path.GetExtension(document.Url);
                    var extractor = ContentExtractorFactory.GetExtractor(extension, _webHostEnvironment.WebRootPath);
                    var imagessupport = pathHelper.IMAGESSUPPORT;
                    var tessLang = pathHelper.TESSSUPPORTLANGUAGES;
                    if (extractor != null)
                    {
                        string tessdataPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, pathHelper.TESSDATA);
                        var content = new DocumentProcessor(extractor).ProcessDocumentByBytes(responseBytes, tessdataPath, pathHelper.TESSSUPPORTLANGUAGES);
                        content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
                        if (!string.IsNullOrEmpty(content))
                        {
                            //await extractSummaryOfDocument(content);
                            string searchIndexPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, pathHelper.SearchIndexPath);
                            var indexWriterManager = new IndexWriterManager(searchIndexPath);
                            indexWriterManager.AddDocument(documentVersion.Id.ToString(), content);
                            indexWriterManager.Commit();
                            indexWriterManager.Dispose();
                        }
                    }
                    else if (Array.Exists(imagessupport, element => element.ToLower() == extension.ToLower()))
                    {
                        this.ExtractTEssData(responseBytes, documentVersion.Id, tessLang);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while indexing document");
                }

            }
            _documentIndexRepository.Remove(documentIndex);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving data");
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding document index content.");
            return true;
        }
    }

    private async Task<bool> extractSummaryOfDocument(string documentText)
    {
        var apiKey = pathHelper.ChatGPTAPIKey;
        var apiURL = pathHelper.ChatGPTAPIURL;
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiURL))
        {
            return await Task.FromResult(true);
        }
        //Chat GPT API Call HttpCLientFactory
        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "gpt-3.5-turbo", // Use "gpt-3.5-turbo" for a cheaper option
            messages = new object[]
            {
                new { role = "system", content = "Summarize the following document briefly." },
                new { role = "user", content = documentText }
            },
            temperature = 0.5
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(apiURL, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            // return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            string summary = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return !string.IsNullOrEmpty(summary);
        }
        else
        {
            var data = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
    }

    private async Task<byte[]> CombineChunkBytes(DocumentVersion documentVersion, Document document, StorageSetting storeageSetting, IStorageService storageService)
    {
        var documentChunks = await documentChunkRepository.All.Where(c => c.DocumentVersionId == documentVersion.Id).OrderBy(c => c.ChunkIndex).ToListAsync();
        var lstBytes = new List<byte[]>();
        foreach (var chunk in documentChunks)
        {
            var fileResult = await storageService.DownloadFileAsync(chunk.Url, storeageSetting.JsonValue, document.Key, document.IV);
            lstBytes.Add(fileResult.FileBytes);
        }
        using (var finalStream = new MemoryStream())
        {
            foreach (var chunk in lstBytes)
            {
                if (chunk != null)
                {
                    finalStream.Write(chunk, 0, chunk.Length);
                }
            }
            return finalStream.ToArray();
        }
    }

    private void ExtractTEssData(byte[] fileBytes, Guid id, string tessLang)
    {
        string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, pathHelper.TESSDATA);
        var tessDataContextExtractor = new TessDataContextExtractor();
        var content = tessDataContextExtractor.ExtractContentByBytes(tessFilePath, fileBytes, tessLang, _logger);
        content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
        if (!string.IsNullOrEmpty(content))
        {
            string searchIndexPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, pathHelper.SearchIndexPath);
            var indexWriterManager = new IndexWriterManager(searchIndexPath);
            indexWriterManager.AddDocument(id.ToString(), content);
            indexWriterManager.Commit();
            indexWriterManager.Dispose();
        }
    }
}
