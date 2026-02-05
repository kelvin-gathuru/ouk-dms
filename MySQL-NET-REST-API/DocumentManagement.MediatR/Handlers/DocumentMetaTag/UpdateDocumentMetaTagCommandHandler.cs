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
public class UpdateDocumentMetaTagCommandHandler(IDocumentMetaTagRepository _documentMetaTagRepository, IUnitOfWork<DocumentContext> _uow, IMapper _mapper) : IRequestHandler<UpdateDocumentMetaTagCommand, ServiceResponse<DocumentMetaTagDto>>
{
    public async Task<ServiceResponse<DocumentMetaTagDto>> Handle(UpdateDocumentMetaTagCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentMetaTagRepository.All
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.Id != request.Id);
        if (entityExist != null)
        {
            return ServiceResponse<DocumentMetaTagDto>.Return409("DocumentMetaTag with same name already exists.");
        }
        entityExist = await _documentMetaTagRepository.All
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (entityExist == null)
        {
            return ServiceResponse<DocumentMetaTagDto>.Return409("DocumentMetaTag does not exists.");
        }
        var entity = _mapper.Map<DocumentMetaTag>(request);
        _documentMetaTagRepository.Update(entity);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<DocumentMetaTagDto>.Return500();
        }
        var entityDto = _mapper.Map<DocumentMetaTagDto>(entity);
        return ServiceResponse<DocumentMetaTagDto>.ReturnResultWith200(entityDto);
    }
}
