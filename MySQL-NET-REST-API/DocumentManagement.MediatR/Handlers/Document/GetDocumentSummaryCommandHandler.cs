using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Document = DocumentManagement.Data.Entities.Document;

namespace DocumentManagement.MediatR.Handlers;
public class GetDocumentSummaryCommandHandler(
     IDocumentRepository _documentRepository,
     IDocumentVersionRepository documentVersionRepository,
     StorageServiceFactory _storeageServiceFactory,
     IStorageSettingRepository _storageSettingRepository,
     IWebHostEnvironment _webHostEnvironment,
     IDocumentChunkRepository documentChunkRepository,
      ILogger<GetDocumentSummaryCommandHandler> _logger,
      ICompanyProfileRepository companyProfileRepository,
      PathHelper pathHelper,
     IHttpClientFactory httpClientFactory,
    Helper.PathHelper _pathHelper) : IRequestHandler<GetDocumentSummaryCommand, ServiceResponse<string>>
{
    public async Task<ServiceResponse<string>> Handle(GetDocumentSummaryCommand request, CancellationToken cancellationToken)
    {
        var documentVersion = documentVersionRepository.All.Where(c => c.DocumentId == request.DocumentId && c.IsCurrentVersion).FirstOrDefault();
        if (documentVersion == null && !documentVersion.IsAllChunkUploaded)
        {
            return ServiceResponse<string>.ReturnFailed(409, "Document is not found.");
        }
        var companyProfile = await companyProfileRepository.All.FirstOrDefaultAsync();
        if (companyProfile == null)
        {
            return ServiceResponse<string>.ReturnFailed(409, "Company profile is not found.");
        }
        //if (string.IsNullOrEmpty(companyProfile.OpenAIAPIKey))
        //{
        //    return ServiceResponse<string>.ReturnFailed(404, "Configure OpenAI key into General Settings.");
        //}

        var summaryFilePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SUMMARYFOLDER, documentVersion.Id.ToString() + ".txt");
        var summary = await ReadSummaryToFile(summaryFilePath);
        if (!string.IsNullOrEmpty(summary))
        {
            return ServiceResponse<string>.ReturnResultWith200(summary);
        }
        var document = await _documentRepository.All.FirstOrDefaultAsync(c => c.Id == documentVersion.DocumentId);
        if (document == null)
        {
            return ServiceResponse<string>.ReturnFailed(409, "Document is not found.");
        }
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);
        if (storeageSetting == null)
        {
            return ServiceResponse<string>.ReturnFailed(409, "Storage setting is not found.");
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
            var imagessupport = _pathHelper.IMAGESSUPPORT;
            var tessLang = _pathHelper.TESSSUPPORTLANGUAGES;
            var content = string.Empty;
            if (extractor != null)
            {
                string tessdataPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
                content = new DocumentProcessor(extractor).ProcessDocumentByBytes(responseBytes, tessdataPath, _pathHelper.TESSSUPPORTLANGUAGES);
                content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
            }
            else if (Array.Exists(imagessupport, element => element.ToLower() == extension.ToLower()))
            {
                content = this.ExtractTEssData(responseBytes, documentVersion.Id, tessLang);
            }
            if (string.IsNullOrEmpty(content))
            {
                return ServiceResponse<string>.ReturnFailed(404, "Content is not found.");
            }
            else
            {

                var contentSummary = await extractSummaryOfDocumentUsingGemini(content, companyProfile);

                if (string.IsNullOrEmpty(contentSummary))
                {
                    return ServiceResponse<string>.ReturnFailed(404, "Content summary is not found.");
                }
                else
                {
                    var summaryFoler = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SUMMARYFOLDER);
                    if (!Directory.Exists(summaryFoler))
                    {
                        Directory.CreateDirectory(summaryFoler);
                    }
                    await this.WriteSummaryToFile(contentSummary, Path.Combine(_webHostEnvironment.WebRootPath, summaryFilePath));

                    return ServiceResponse<string>.ReturnResultWith200(contentSummary);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while indexing document");
        }

        return ServiceResponse<string>.ReturnResultWith200("");
    }

    private async Task<string> ReadSummaryToFile(string filePath)
    {
        // Read the summary from the file
        if (!File.Exists(filePath))
        {
            return string.Empty;
        }
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string summary = await reader.ReadToEndAsync();
                return summary.Trim();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading summary from file");
            return string.Empty;
        }
    }

    private async Task<bool> WriteSummaryToFile(string summary, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync(summary);
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing summary to file");
            return false;
        }
    }
    private async Task<string> extractSummaryOfDocumentUsingGemini(string documentText, CompanyProfile companyProfile)
    {
        var apiKey = pathHelper.GEMINI_APIKEY;
        var googleAI = new GoogleAI(apiKey: apiKey);
        var model = googleAI.GenerativeModel(model: Model.Gemini15Flash);

        int chunkSize = 16000;

        if (documentText.Length <= chunkSize)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                // 🔹 Small document → single API call
                string prompt = $"Please summarize the following document briefly:\n\n{documentText}";
                var response = await model.GenerateContent(prompt, cancellationToken: cts.Token);
                return response.Text.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        try
        {
            // 🔹 Large document → chunking
            var chunks = new List<string>();
            for (int i = 0; i < documentText.Length; i += chunkSize)
            {
                chunks.Add(documentText.Substring(i, Math.Min(chunkSize, documentText.Length - i)));
            }

            var partialSummaries = new List<string>();
            var summaryTasks = chunks.Select(async chunk =>
            {
                string prompt = $"Summarize this part of the document briefly:\n\n{chunk}";
                var response = await model.GenerateContent(prompt);
                return response.Text?.Trim() ?? "";
            }).ToArray();

            string combinedSummaries = string.Join("\n\n",
         (await Task.WhenAll(summaryTasks))
         .Where(s => !string.IsNullOrWhiteSpace(s)));

            // 🔹 Combine chunk summaries and get final summary

            string finalPrompt = $"Here are summaries of different parts of a document:\n\n{combinedSummaries}\n\nPlease create a single concise summary of the entire document.";
            var finalResponse = await model.GenerateContent(finalPrompt);

            return finalResponse.Text.Trim();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //private async Task<string> extractSummaryOfDocument(string documentText, CompanyProfile companyProfile)
    //{
    //    var apiKey = companyProfile.OpenAIAPIKey;
    //    var apiURL = pathHelper.ChatGPTAPIURL;
    //    if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiURL))
    //    {
    //        return "";
    //    }
    //    //Chat GPT API Call HttpCLientFactory
    //    var client = httpClientFactory.CreateClient();
    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

    //    var requestBody = new
    //    {
    //        model = "gpt-3.5-turbo", // Use "gpt-3.5-turbo" for a cheaper option
    //        messages = new object[]
    //        {
    //            new { role = "system", content = "Summarize the following document briefly." },
    //            new { role = "user", content = documentText }
    //        },
    //        temperature = 0.5
    //    };

    //    var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
    //    HttpResponseMessage response = await client.PostAsync(apiURL, content);
    //    if (response.IsSuccessStatusCode)
    //    {
    //        string jsonResponse = await response.Content.ReadAsStringAsync();
    //        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
    //        string summary = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    //        return summary;
    //    }
    //    else
    //    {
    //        var data = await response.Content.ReadAsStringAsync();
    //        _logger.LogError($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
    //        return "";
    //    }
    //}
    private async Task<string> ExtractSummaryOfDocumentOPENAI(string documentText, CompanyProfile companyProfile)
    {
        var apiKey = companyProfile.OpenAIAPIKey;
        var apiURL = pathHelper.ChatGPTAPIURL;

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiURL))
            return "";

        int chunkSize = 2000; // adjust based on token limits
        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        // Helper method to call OpenAI API for a single text
        async Task<string> GetSummary(string text)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new object[]
                {
                new { role = "system", content = "Summarize the following document briefly." },
                new { role = "user", content = text }
                },
                temperature = 0.5
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(apiURL, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error: {response.StatusCode} - {error}");
                return "";
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            return doc.RootElement.GetProperty("choices")[0]
                          .GetProperty("message")
                          .GetProperty("content")
                          .GetString() ?? "";
        }

        // 🔹 Single call for small documents
        if (documentText.Length <= chunkSize)
            return await GetSummary(documentText);

        // 🔹 Chunking for large documents
        var chunks = new List<string>();
        for (int i = 0; i < documentText.Length; i += chunkSize)
            chunks.Add(documentText.Substring(i, Math.Min(chunkSize, documentText.Length - i)));

        // 🔹 Summarize all chunks in parallel
        var summaryTasks = chunks.Select(chunk => GetSummary(chunk)).ToArray();
        var partialSummaries = (await Task.WhenAll(summaryTasks))
                                    .Where(s => !string.IsNullOrWhiteSpace(s))
                                    .Select(s => s.Trim())
                                    .ToList();

        // 🔹 Combine all partial summaries and get final summary
        string combinedSummaries = string.Join("\n\n", partialSummaries);
        string finalPrompt = $"Here are summaries of different parts of a document:\n\n{combinedSummaries}\n\nPlease create a single concise summary of the entire document.";

        return await GetSummary(finalPrompt);
    }
    private async Task<string> GetSummary(string text, HttpClient client, string apiURL)
    {
        var requestBody = new
        {
            model = "gpt-3.5-turbo", // or "gpt-4" if needed
            messages = new object[]
            {
            new { role = "system", content = "Summarize the following document briefly." },
            new { role = "user", content = text }
            },
            temperature = 0.5
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(apiURL, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Error: {response.StatusCode} - {error}");
            return "";
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
        return doc.RootElement
                  .GetProperty("choices")[0]
                  .GetProperty("message")
                  .GetProperty("content")
                  .GetString() ?? "";
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
                finalStream.Write(chunk, 0, chunk.Length);
            }
            return finalStream.ToArray();
        }
    }

    private string ExtractTEssData(byte[] fileBytes, Guid id, string tessLang)
    {
        string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
        var tessDataContextExtractor = new TessDataContextExtractor();
        var content = tessDataContextExtractor.ExtractContentByBytes(tessFilePath, fileBytes, tessLang, _logger);
        content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
        return content;
    }
}
