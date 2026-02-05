using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;
public class OCRContentExtractorCommandHandler(
     IWebHostEnvironment _webHostEnvironment,
      ILogger<AddDocumentIndexContentCommandHandler> _logger,
    Helper.PathHelper _pathHelper) : IRequestHandler<OCRContentExtractorCommand, ServiceResponse<string>>
{
    public async Task<ServiceResponse<string>> Handle(OCRContentExtractorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!FileSignatureHelper.IsFileSignatureValid(request.File))
            {
                return ServiceResponse<string>.ReturnFailed(409, "Invalid file is not valid.");
            }
            string extension = request.Extension;
            var extractor = ContentExtractorFactory.GetExtractor(extension, _webHostEnvironment.WebRootPath);
            var imagessupport = _pathHelper.IMAGESSUPPORT;
            var tessLang = _pathHelper.TESSSUPPORTLANGUAGES;
            if (extractor != null)
            {
                string tessdataPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
                var bytes = AesOperation.ConvertIFormFileToByteArray(request.File);
                var content = extractor.ExtractContentByBytes(bytes, tessdataPath, _pathHelper.TESSSUPPORTLANGUAGES);
                //  content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
                return ServiceResponse<string>.ReturnResultWith200(content);
            }
            else if (Array.Exists(imagessupport, element => element.ToLower() == "." + extension.ToLower() || element.ToLower() == extension.ToLower()))
            {
                var content = await this.ExtractTEssData(request.File, tessLang);
                //content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
                return ServiceResponse<string>.ReturnResultWith200(content);
            }
            return ServiceResponse<string>.ReturnResultWith200("");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while OCR Content");
            return ServiceResponse<string>.ReturnFailed(409, "Invalid file signature.");
        }
    }

    private async Task<string> ExtractTEssData(IFormFile file, string tessLang)
    {
        string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
        var tessDataContextExtractor = new TessDataContextExtractor();
        var content = await tessDataContextExtractor.ExtractContentByFile(tessFilePath, file, tessLang, _logger);
        content = CleanOCRText.ClearText(content);
        return content;
    }
}
