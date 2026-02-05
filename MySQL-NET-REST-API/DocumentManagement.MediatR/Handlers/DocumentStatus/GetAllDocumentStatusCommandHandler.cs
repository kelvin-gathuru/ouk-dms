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
    public class GetAllDocumentStatusCommandHandler(IDocumentStatusRepository _documentStatusRepository, IMapper _mapper) : IRequestHandler<GetAllDocumentStatusQuery, ServiceResponse<List<DocumentStatusDto>>>
    {

        public async Task<ServiceResponse<List<DocumentStatusDto>>> Handle(GetAllDocumentStatusQuery request, CancellationToken cancellationToken)
        {
            var entities = await _documentStatusRepository.All.ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<DocumentStatusDto>>(entities);
            return ServiceResponse<List<DocumentStatusDto>>.ReturnResultWith200(dtos);
        }
    }
}
