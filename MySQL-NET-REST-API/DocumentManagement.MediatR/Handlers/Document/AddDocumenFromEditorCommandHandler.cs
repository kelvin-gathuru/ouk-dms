using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class AddDocumenFromEditorCommandHandler(
    PathHelper _pathHelper,
    StorageServiceFactory _storeageServiceFactory,
    IStorageSettingRepository _storageSettingRepository
 ) : IRequestHandler<AddDocumenFromEditorCommand, ServiceResponse<DocumentUrl>>
{
    public async Task<ServiceResponse<DocumentUrl>> Handle(AddDocumenFromEditorCommand request, CancellationToken cancellationToken)
    {
        if (request.Upload == null)
        {
            return ServiceResponse<DocumentUrl>.ReturnFailed(409, "Please select the file.");
        }

        if (!FileSignatureHelper.IsFileSignatureValid(request.Upload))
        {
            return ServiceResponse<DocumentUrl>.ReturnFailed(409, "Invalid file signature.");
        }
        long fileSizeInBytes = request.Upload.Length;
        // Convert file size to kilobytes or megabytes if necessary
        double fileSizeInKB = fileSizeInBytes / 1024.0;
        var extension = Path.GetExtension(request.Upload.FileName);


        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(Guid.Empty);

        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

        var fileNameKeyValut = await storageService.UploadFileAsync(request.Upload, storeageSetting, extension);

        var path = _pathHelper.DocumentPath + "/" + fileNameKeyValut.FileName;
        var documentUrl = new DocumentUrl
        {
            Url = path,
        };
        return ServiceResponse<DocumentUrl>.ReturnResultWith200(documentUrl);
    }
}
