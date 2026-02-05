using System;
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
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateFileRequestCommandHandler(IAllowFileExtensionRepository _allowFileExtensionRepository, IFileRequestsRepository _fileRequestsRepository, IUnitOfWork<DocumentContext> _uow, IMapper _mapper) : IRequestHandler<UpdateFileRequestCommand, ServiceResponse<FileRequestDto>>
{
    public async Task<ServiceResponse<FileRequestDto>> Handle(UpdateFileRequestCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _fileRequestsRepository.All
            .FirstOrDefaultAsync(c => c.Subject == request.Subject && c.Id != request.Id);
        if (entityExist != null)
        {
            return ServiceResponse<FileRequestDto>.Return409("File request with same already exists.");
        }
        entityExist = await _fileRequestsRepository.All
            .FirstOrDefaultAsync(c => c.Id == request.Id);
        if (entityExist == null)
        {
            return ServiceResponse<FileRequestDto>.Return409("File request does not exists.");
        }
        string fileExtensions = "";
        foreach (var item in request.FileExtension)
        {
            var fileExtension = _allowFileExtensionRepository.FindBy(c => c.FileType == (FileType)item).FirstOrDefault();
            if (fileExtension != null)
            {
                fileExtensions += fileExtension.FileType + ",";
            }
        }
        if (!string.IsNullOrEmpty(fileExtensions))
        {
            fileExtensions = fileExtensions.TrimEnd(',');
        }
        if (request.Password != null)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                entityExist.Password = null;
            }
            else
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(request.Password);
                entityExist.Password = Convert.ToBase64String(plainTextBytes);
            }
        }
        entityExist.Subject = request.Subject;
        entityExist.Email = request.Email;
        entityExist.MaxDocument = request.MaxDocument;
        entityExist.LinkExpiryTime = request.LinkExpiryTime;
        entityExist.SizeInMb = request.SizeInMb;
        entityExist.AllowExtension = fileExtensions;
        _fileRequestsRepository.Update(entityExist);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<FileRequestDto>.Return500();
        }
        var entityDto = _mapper.Map<FileRequestDto>(entityExist);
        if (!string.IsNullOrWhiteSpace(entityDto.Password))
        {
            var base64EncodedBytes = Convert.FromBase64String(entityDto.Password);
            entityDto.Password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        return ServiceResponse<FileRequestDto>.ReturnResultWith200(entityDto);
    }
}
