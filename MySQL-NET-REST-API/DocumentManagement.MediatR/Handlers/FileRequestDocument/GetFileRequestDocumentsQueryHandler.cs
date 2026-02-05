using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetFileRequestDocumentsQueryHandler (IFileRequestDocumentRepository _fileRequestDocumentRepository,IMapper _mapper) : IRequestHandler<GetFileRequestDocumentsQuery, ServiceResponse<List<FileRequestDocumentDto>>>
    {
        public async Task<ServiceResponse<List<FileRequestDocumentDto>>> Handle(GetFileRequestDocumentsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _fileRequestDocumentRepository.FindBy(c => c.FileRequestId == request.Id).ToListAsync();
            if (entity == null)
            {
                return ServiceResponse<List<FileRequestDocumentDto>>.Return409("Not found");
            }
            var entityDto = _mapper.Map<List<FileRequestDocumentDto>>(entity);
            return ServiceResponse<List<FileRequestDocumentDto>>.ReturnResultWith200(entityDto);
        }
    }
}
