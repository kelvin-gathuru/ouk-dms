using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace DocumentManagement.MediatR.Handlers
{
    public class DownloadFileRequestDocumentCommandHandler(IMediator mediator, IWebHostEnvironment _webHostEnvironment, PathHelper _pathHelper)
        : IRequestHandler<DownloadFileRequestDocumentCommand, ServiceResponse<DocumentDownload>>
    {
        private string StoragePath => Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.FileRequestPath);

        private byte[] GetFileBytes(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return AesOperation.ConvertStreamToByteArray(stream);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read file: {ex.Message}", ex);
            }
        }
        public async Task<ServiceResponse<DocumentDownload>> Handle(DownloadFileRequestDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    return ServiceResponse<DocumentDownload>.ReturnFailed(400, "Invalid request.");
                }
                var command = new GetFileRequestDocumentQuery { Id = request.Id };
                var result = await mediator.Send(command);

                if (!result.Success || result.Data == null)
                {
                    return ServiceResponse<DocumentDownload>.ReturnFailed(404, result.Errors);
                }

                var entities = result.Data;
                string filePath = Path.Combine(StoragePath, entities.Url);
                byte[] fileBytes = GetFileBytes(filePath);
                return ServiceResponse<DocumentDownload>.ReturnResultWith200(new DocumentDownload
                {
                    Data = fileBytes,
                    FileName = entities.Name,
                    ContentType = FileHelper.GetMimeType(entities.Url),
                });
            }
            catch (Exception ex)
            {
                return ServiceResponse<DocumentDownload>.ReturnFailed(400, ex.Message);
            }
        }
    }
}

