using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetFileRequestDocumentQueryHandler (IFileRequestDocumentRepository _fileRequestDocumentRepository,IMapper _mapper): IRequestHandler<GetFileRequestDocumentQuery, ServiceResponse<FileRequestDocumentDto>>
    {
        public async Task<ServiceResponse<FileRequestDocumentDto>> Handle(GetFileRequestDocumentQuery request, CancellationToken cancellationToken)
        {
            var entity = await _fileRequestDocumentRepository.All
                .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entity == null)
            {
                return ServiceResponse<FileRequestDocumentDto>.Return404();
            }

            var entityDto = _mapper.Map<FileRequestDocumentDto>(entity);
            return ServiceResponse<FileRequestDocumentDto>.ReturnResultWith200(entityDto);
        }
    }
}
