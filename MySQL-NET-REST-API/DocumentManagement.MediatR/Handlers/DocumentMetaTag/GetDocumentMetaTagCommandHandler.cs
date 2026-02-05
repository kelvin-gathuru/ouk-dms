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
public class GetDocumentMetaTagCommandHandler(IDocumentMetaTagRepository _documentMetaTagRepository, IMapper _mapper) : IRequestHandler<GetDocumentMetaTagCommand, ServiceResponse<DocumentMetaTagDto>>
{
    public async Task<ServiceResponse<DocumentMetaTagDto>> Handle(GetDocumentMetaTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _documentMetaTagRepository.All
           .FirstOrDefaultAsync(w => w.Id == request.Id);
        if (entity == null)
        {
            return ServiceResponse<DocumentMetaTagDto>.Return409("Not found");
        }
        var entityDto = _mapper.Map<DocumentMetaTagDto>(entity);
        return ServiceResponse<DocumentMetaTagDto>.ReturnResultWith200(entityDto);
    }
}
