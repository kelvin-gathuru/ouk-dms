using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout.Element;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddDocumentSignatureCommandHandler(
    IUnitOfWork<DocumentContext> _uow,
    IDocumentSignatureRepository _documentSignatureRepository,
    IDocumentRepository _documentRepository,
    IDocumentVersionRepository documentVersionRepository,
    IUserRepository _userRepository,
    ICompanyProfileRepository _companyProfileRepository,
    IMediator _mediator,
    IMapper _mapper,
    UserInfoToken _userInfoToken,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
    IStorageSettingRepository _storageSettingRepository,
     StorageServiceFactory _storeageServiceFactory,
   IDocumentChunkRepository documentChunkRepository,
    IConnectionMappingRepository connectionMappingRepository,
    UserInfoToken userInfoToken,
     IHubContext<UserHub, IHubClient> hubContext,
     ILogger<AddDocumentSignatureCommandHandler> logger,
     ICategoryRepository categoryRepository) : IRequestHandler<AddDocumentSignatureCommand, ServiceResponse<DocumentSignatureDto>>
{

    private readonly string _storagePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SignaturePath);

    private int CountImagesOnPage(PdfPage page)
    {
        var resources = page.GetResources();
        var xObjects = resources.GetResource(PdfName.XObject);
        if (xObjects == null) return 0;

        int imageCount = 0;
        foreach (var entry in xObjects.EntrySet())
        {
            var xObject = entry.Value;
            if (xObject.IsStream())
            {
                var stream = (PdfStream)xObject;
                var subtype = stream.GetAsName(PdfName.Subtype);
                if (PdfName.Image.Equals(subtype))
                {
                    imageCount++;
                }
            }
        }
        return imageCount;
    }
    private int getSignatureCount(string content, string keyword)
    {
        int index = 0;
        int count = 0;
        while ((index = content.IndexOf(keyword, index)) != -1)
        {
            count++;
            index += keyword.Length;
        }
        return count;
    }
    private byte[] AddSignatureToPdf(byte[] pdfBytes, string signatureImagePath, string signedBy)
    {
        try
        {
            using (var pdfReaderStream = new MemoryStream(pdfBytes))
            using (var pdfWriterStream = new MemoryStream())
            {
                var pdfReader = new PdfReader(pdfReaderStream);
                var pdfWriter = new PdfWriter(pdfWriterStream);
                var pdfDoc = new PdfDocument(pdfReader, pdfWriter);
                var document = new iText.Layout.Document(pdfDoc);

                float boxWidth = 105;
                float boxHeight = 55;
                float margin = 10;
                int maxColumns = 5;

                float startX = margin;
                float startY = margin;

                int currentPageNumber = 1;
                var page = pdfDoc.GetPage(currentPageNumber);
                float pageHeight = page.GetPageSize().GetHeight();
                //int signaturesPerPage = CountImagesOnPage(page); // Adjust if needed
                var strategy = new SimpleTextExtractionStrategy();
                string content = PdfTextExtractor.GetTextFromPage(page, strategy);
                int signaturesPerPage = string.IsNullOrWhiteSpace(content) ? 0 : getSignatureCount(content, "Digitally signed By");
                float currentX = 0;
                float currentY = 0;
                while (true)
                {
                    int row = 0;
                    int column = 0;
                    if (signaturesPerPage <= maxColumns)
                    {
                        row = signaturesPerPage / maxColumns;
                        column = signaturesPerPage % maxColumns;
                        currentX = (column * (boxWidth + margin)) + startX;
                        currentY = startY + (row * (boxHeight + margin));
                    }
                    else
                    {
                        row = (signaturesPerPage - maxColumns) / 2 + 1;
                        column = (signaturesPerPage - maxColumns) % 2 > 0 ? maxColumns - 1 : 0;
                        var marginSpace = column > 0 ? 25 : margin;
                        currentX = (column * (boxWidth + margin)) + marginSpace;
                        currentY = startY + (row * (boxHeight + margin));
                    }

                    // Check if this row goes beyond page height
                    if (currentY + boxHeight + margin > pageHeight)
                    {
                        // Add or switch to new page
                        if (currentPageNumber == pdfDoc.GetNumberOfPages())
                        {
                            pdfDoc.AddNewPage();
                        }

                        currentPageNumber++;
                        page = pdfDoc.GetPage(currentPageNumber);
                        pageHeight = page.GetPageSize().GetHeight();
                        signaturesPerPage = 0; // Reset for new page
                        continue;
                    }

                    // Draw the signature box
                    var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);
                    canvas.SetLineWidth(0.8f);
                    canvas.SetStrokeColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                    canvas.Rectangle(currentX, currentY, boxWidth, boxHeight);
                    canvas.Stroke();
                    var boldFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                    var signedByStamp = new iText.Layout.Element.Paragraph("Digitally signed By")
                       .SetFontSize(8)
                       .SetFont(boldFont)
                       .SetFontColor(iText.Kernel.Colors.ColorConstants.RED)
                       .SetFixedPosition(currentX + 10, currentY + 40, boxWidth - 20)
                       .SetPageNumber(currentPageNumber);
                    document.Add(signedByStamp);

                    var signatureImage = new Image(ImageDataFactory.Create(signatureImagePath));
                    signatureImage.SetFixedPosition(currentX + 5, currentY + 25, boxWidth - 10);
                    document.Add(signatureImage.SetPageNumber(currentPageNumber));

                    var signedByText = new iText.Layout.Element.Paragraph(signedBy)
                        .SetFontSize(7)
                         .SetFont(boldFont)
                        .SetFixedPosition(currentX + 10, currentY + 10, boxWidth - 20)
                        .SetPageNumber(currentPageNumber);
                    document.Add(signedByText);
                    var italicFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ITALIC);
                    var signDateText = new iText.Layout.Element.Paragraph($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .SetFontSize(7)
                        .SetFont(italicFont)
                        .SetFixedPosition(currentX + 10, currentY, boxWidth - 20)
                        .SetPageNumber(currentPageNumber);
                    document.Add(signDateText);

                    signaturesPerPage++;
                    break;
                }


                document.Close();

                var updatedPdfBytes = pdfWriterStream.ToArray();
                return updatedPdfBytes;

            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to write file: {ex.Message}", ex);
        }
    }

    public async Task<ServiceResponse<DocumentSignatureDto>> Handle(AddDocumentSignatureCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var fileName = $"{Guid.NewGuid()}_signature.png";
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            var base64Data = request.SignatureUrl.Split(',')[1];
            var imageBytes = Convert.FromBase64String(base64Data);
            string fullPath = Path.Combine(_storagePath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
            }
            var entity = _mapper.Map<DocumentSignature>(request);
            entity.Id = Guid.NewGuid();
            entity.SignatureUrl = fileName;
            entity.SignatureUserId = _userInfoToken.Id;
            entity.SignatureDate = DateTime.UtcNow;
            _documentSignatureRepository.Add(entity);
            var document = await _documentRepository.FindAsync(request.DocumentId);
            var documentVersion = documentVersionRepository.All.Where(c => c.DocumentId == request.DocumentId && c.IsCurrentVersion).FirstOrDefault();
            if (document != null)
            {
                document.SignById = _userInfoToken.Id;
                document.SignDate = DateTime.UtcNow;
                document.IsSignatureExists = true;
                _documentRepository.Update(document);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<DocumentSignatureDto>.Return500();
            }
            var category = categoryRepository.All.Where(c => c.Id == document.CategoryId).FirstOrDefault();
            try
            {
                var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
                if (onlineUsers.Count() > 0)
                {
                    var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                    if (user != null)
                    {
                        await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                    }
                    else
                    {
                        await hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SignalR Error");
            }
            if (document != null)
            {
                var companyProfile = _companyProfileRepository.All.FirstOrDefault();
                var extension = document.Url.Split('.');
                byte[] responseBytes;
                if ((extension[1].ToLower() == "pdf" || extension[1].ToLower() == ".pdf") && companyProfile.AllowPdfSignature)
                {
                    if (documentVersion.IsChunk)
                    {
                        responseBytes = await CombineChunkBytes(documentVersion, document);
                    }
                    else
                    {
                        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);

                        if (storeageSetting == null)
                        {
                            return null;
                        }

                        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

                        var fileResult = await storageService.DownloadFileAsync(documentVersion.Url, storeageSetting.JsonValue, document.Key, document.IV);

                        responseBytes = fileResult.FileBytes;
                    }

                    var user = await _userRepository.FindAsync(_userInfoToken.Id);
                    var singedBy = string.Empty;
                    if (user != null)
                    {
                        singedBy = $"{user.FirstName} {user.LastName}";
                    }

                    var updatedPdfBytes = AddSignatureToPdf(responseBytes, fullPath, singedBy);

                    double sizeInMB = updatedPdfBytes.Length / (1024.0 * 1024.0);

                    if (sizeInMB > 5)
                    {
                        var chunks = SplitBytes(updatedPdfBytes, 2 * 1024 * 1024);
                        var i = 0;

                        var version = new DocumentVersion
                        {
                            Id = Guid.NewGuid(),
                            DocumentId = document.Id,
                            Url = documentVersion.Url,
                            Key = documentVersion.Key,
                            IV = documentVersion.IV,
                            IsCurrentVersion = true,
                            VersionNumber = await documentVersionRepository.GetDocumentVersionCount(document.Id) + 1,
                            CreatedBy = _userInfoToken.Id,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedBy = _userInfoToken.Id,
                            ModifiedDate = DateTime.UtcNow,
                            SignById = _userInfoToken.Id,
                            SignDate = DateTime.UtcNow,
                            Comment = documentVersion.Comment,
                            Extension = documentVersion.Extension,
                            IsChunk = true,
                            IsAllChunkUploaded = true
                        };
                        documentVersion.IsCurrentVersion = false;
                        documentVersionRepository.Update(documentVersion);
                        documentVersionRepository.Add(version);
                        document.IsChunk = true;
                        document.Url = version.Url;
                        document.Key = version.Key;
                        document.IV = version.IV;
                        document.IsChunk = true;
                        document.IsAllChunkUploaded = true;
                        document.Extension = version.Extension;
                        document.IsAddedPageIndxing = false;
                        _documentRepository.Update(document);

                        if (await _uow.SaveAsync() <= 0)
                        {
                            return ServiceResponse<DocumentSignatureDto>.Return500();
                        }

                        foreach (var chunk in chunks)
                        {
                            var formFile = ConvertByteArrayToIFormFile(chunk, "SignedDocument.pdf", "application/pdf");
                            var uploadDocumentChunkCommand = new UploadDocumentChunkCommand
                            {
                                DocumentVersionId = version.Id,
                                Extension = "pdf",
                                File = formFile,
                                Size = chunk.Length,
                                ChunkIndex = i,
                                TotalChunks = chunks.Count()
                            };
                            var result = await _mediator.Send(uploadDocumentChunkCommand);
                            i++;
                        }
                    }
                    else
                    {

                        var formFile = ConvertByteArrayToIFormFile(updatedPdfBytes, "SignedDocument.pdf", "application/pdf");
                        var uploadNewDocumentVersionCommand = new UploadNewDocumentVersionCommand
                        {
                            File = formFile,
                            DocumentId = document.Id,
                            Extension = extension[1],
                            IsSignatureExists = document.IsSignatureExists
                        };
                        var result = await _mediator.Send(uploadNewDocumentVersionCommand);
                    }
                }
            }
            var entityDto = _mapper.Map<DocumentSignatureDto>(entity);
            return ServiceResponse<DocumentSignatureDto>.ReturnResultWith201(entityDto);
        }
        catch (Exception ex)
        {
            return ServiceResponse<DocumentSignatureDto>.Return409($"Failed to save signature: {ex.Message}");
        }

    }


    private List<byte[]> SplitBytes(byte[] source, int chunkSize)
    {
        List<byte[]> chunks = new List<byte[]>();
        int totalLength = source.Length;

        for (int i = 0; i < totalLength; i += chunkSize)
        {
            int currentChunkSize = Math.Min(chunkSize, totalLength - i);
            byte[] chunk = new byte[currentChunkSize];
            Array.Copy(source, i, chunk, 0, currentChunkSize);
            chunks.Add(chunk);
        }

        return chunks;
    }

    private IFormFile ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName, string contentType)
    {
        var stream = new MemoryStream(fileBytes); // Do not use 'using' to keep stream open
        var formFile = new FormFile(stream, 0, fileBytes.Length, null, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
        return formFile;
    }


    private async Task<byte[]> CombineChunkBytes(DocumentVersion documentVersion, Document document)
    {
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);

        if (storeageSetting == null)
        {
            return null;
        }

        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
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

}
