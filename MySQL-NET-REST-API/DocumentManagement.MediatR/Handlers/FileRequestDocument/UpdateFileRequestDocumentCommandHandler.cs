using System;
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

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateFileRequestDocumentCommandHandler (IFileRequestDocumentRepository _fileRequestDocumentRepository, UserInfoToken _userInfoToken, IUnitOfWork<DocumentContext> _uow,IMapper _mapper) : IRequestHandler<UpdateFileRequestDocumentCommand, ServiceResponse<FileRequestDocumentDto>>
    {
        public async Task<ServiceResponse<FileRequestDocumentDto>> Handle(UpdateFileRequestDocumentCommand request, CancellationToken cancellationToken)
        { 
            var entityExist = await _fileRequestDocumentRepository.All
                .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<FileRequestDocumentDto>.Return409("File request document does not exists.");
            }
            entityExist.Reason = request.Reason;
            entityExist.FileRequestDocumentStatus = FileRequestDocumentStatus.REJECTED;
            entityExist.ApprovalOrRjectedById = _userInfoToken.Id;
            entityExist.ApprovedRejectedDate = DateTime.UtcNow;
            _fileRequestDocumentRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<FileRequestDocumentDto>.Return500();
            }
            var entityDto = _mapper.Map<FileRequestDocumentDto>(entityExist);
           
            return ServiceResponse<FileRequestDocumentDto>.ReturnResultWith200(entityDto);
        }
    }
}
