using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DocumentManagement.MediatR.Handlers;
public class AddDocumentSignatureWithPositionCommandHandler(
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
     IHubContext<UserHub, IHubClient> hubContext,
     ILogger<AddDocumentSignatureCommandHandler> logger,
     ICategoryRepository categoryRepository) : IRequestHandler<AddDocumentSignatureWithPositionCommand, ServiceResponse<DocumentSignatureDto>>
{

    private readonly string _storagePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SignaturePath);

    //private int CountImagesOnPage(PdfPage page)
    //{
    //    var resources = page.GetResources();
    //    var xObjects = resources.GetResource(PdfName.XObject);
    //    if (xObjects == null) return 0;

    //    int imageCount = 0;
    //    foreach (var entry in xObjects.EntrySet())
    //    {
    //        var xObject = entry.Value;
    //        if (xObject.IsStream())
    //        {
    //            var stream = (PdfStream)xObject;
    //            var subtype = stream.GetAsName(PdfName.Subtype);
    //            if (PdfName.Image.Equals(subtype))
    //            {
    //                imageCount++;
    //            }
    //        }
    //    }
    //    return imageCount;
    //}

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

    private byte[] AddSignatureToPdfNew(byte[] pdfBytes, AddDocumentSignatureWithPositionCommand request, string signatureImagePath, string signedBy)
    {
        try
        {
            using (var pdfReaderStream = new MemoryStream(pdfBytes))
            using (var pdfWriterStream = new MemoryStream())
            {
                PdfReader pdfReader;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    // ✅ Open with password
                    var readerProps = new ReaderProperties()
                        .SetPassword(Encoding.UTF8.GetBytes(request.Password));
                    pdfReader = new PdfReader(pdfReaderStream, readerProps);
                }
                else
                {
                    // ✅ Open without password
                    pdfReader = new PdfReader(pdfReaderStream);
                }
                var pdfWriter = new PdfWriter(pdfWriterStream);
                var pdfDoc = new PdfDocument(pdfReader, pdfWriter, new StampingProperties().UseAppendMode());

                int currentPageNumber = request.PageNumber;
                var page = pdfDoc.GetPage(currentPageNumber);
                float pageHeight = page.GetPageSize().GetHeight();

                float boxWidth = 105;
                float boxHeight = 60;

                float x = request.XAxis;
                float y = 0;
                if (string.IsNullOrEmpty(request.Password))
                {
                    y = pageHeight - request.YAxis;
                }
                else
                {
                    y = request.YAxis - 15;
                }

                // ✅ Create PdfCanvas overlay
                var pdfCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                var pageSize = page.GetPageSize();
                var canvas = new iText.Layout.Canvas(pdfCanvas, pageSize);

                // ✅ Load and draw signature image
                byte[] imageBytes = File.ReadAllBytes(signatureImagePath);
                var imgData = ImageDataFactory.Create(imageBytes);

                var signatureImage = new Image(imgData)
                    .ScaleToFit(200, 80) //
                    .SetFixedPosition(x, y - 15, 200);

                canvas.Add(signatureImage);

                // ✅ Fonts
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // ✅ Signed by text (just below the image)
                var signedByText = new Paragraph($"Signed by: {signedBy}")
                    .SetFont(boldFont)
                    .SetFontSize(6)
                    .SetFontColor(ColorConstants.RED)
                    .SetFixedPosition(x, y - 30, boxWidth);
                canvas.Add(signedByText);

                // ✅ Date text (below "Signed by")
                var signDateText = new Paragraph($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                    .SetFontSize(5)
                    .SetFontColor(ColorConstants.RED)
                    .SetFixedPosition(x, y - 40, boxWidth);
                canvas.Add(signDateText);

                canvas.Close();
                pdfDoc.Close();

                return pdfWriterStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to write file: {ex.Message}", ex);
        }
    }
    public async Task<ServiceResponse<DocumentSignatureDto>> Handle(AddDocumentSignatureWithPositionCommand request, CancellationToken cancellationToken)
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
                var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = _userInfoToken.Id.ToString() });
                if (onlineUsers.Count() > 0)
                {
                    var user = connectionMappingRepository.GetUserInfoById(_userInfoToken.Id);
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
                if ((extension[1].ToLower() == "pdf" || extension[1].ToLower() == ".pdf"))
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

                    var updatedPdfBytes = AddSignatureToPdfNew(responseBytes, request, fullPath, singedBy);

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


    private async Task<byte[]> CombineChunkBytes(DocumentVersion documentVersion, Data.Entities.Document document)
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

