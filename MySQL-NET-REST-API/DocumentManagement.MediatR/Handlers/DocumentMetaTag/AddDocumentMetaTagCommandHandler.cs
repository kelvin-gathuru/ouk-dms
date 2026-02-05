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

namespace DocumentManagement.MediatR.Handlers;
public class AddDocumentMetaTagCommandHandler(IDocumentMetaTagRepository _documentMetaTagRepository, IMapper _mapper, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddDocumentMetaTagCommand, ServiceResponse<DocumentMetaTagDto>>
{
    public async Task<ServiceResponse<DocumentMetaTagDto>> Handle(AddDocumentMetaTagCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentMetaTagRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            return ServiceResponse<DocumentMetaTagDto>.Return409("DocumentMetaTag with same name already exists.");
        }
        var entity = _mapper.Map<DocumentMetaTag>(request);
        entity.Id = Guid.NewGuid();
        _documentMetaTagRepository.Add(entity);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<DocumentMetaTagDto>.Return500();
        }
        var entityDto = _mapper.Map<DocumentMetaTagDto>(entity);
        return ServiceResponse<DocumentMetaTagDto>.ReturnResultWith201(entityDto);
    }
}