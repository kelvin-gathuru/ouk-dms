using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;
public class GetAllDocumentMetaTagCommandHandler(IDocumentMetaTagRepository _documentMetaTagRepository, IMapper _mapper) : IRequestHandler<GetAllDocumentMetaTagCommand, ServiceResponse<List<DocumentMetaTagDto>>>
{
    public async Task<ServiceResponse<List<DocumentMetaTagDto>>> Handle(GetAllDocumentMetaTagCommand request, CancellationToken cancellationToken)
    {
        var entities = await _documentMetaTagRepository.All
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync(cancellationToken);
        if (entities.Count == 0)
        {
            return ServiceResponse<List<DocumentMetaTagDto>>.ReturnResultWith200([]);
        }
        var entityDtos = _mapper.Map<List<DocumentMetaTagDto>>(entities);
        return ServiceResponse<List<DocumentMetaTagDto>>.ReturnResultWith200(entityDtos);
    }
}
