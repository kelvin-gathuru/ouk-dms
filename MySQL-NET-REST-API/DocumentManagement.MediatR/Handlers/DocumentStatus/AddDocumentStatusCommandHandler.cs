using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddDocumentStatusCommandHandler(IDocumentStatusRepository _documentStatusRepository,IMapper _mapper, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddDocumentStatusCommand, ServiceResponse<DocumentStatusDto>>
    {
        public async Task<ServiceResponse<DocumentStatusDto>> Handle(AddDocumentStatusCommand request, CancellationToken cancellationToken)
        {

            var entityExist = await _documentStatusRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<DocumentStatusDto>.Return409("DocumentStatus Name already exists");
            }
            var entity = _mapper.Map<DocumentStatus>(request);
            entity.Id = Guid.NewGuid();
            _documentStatusRepository.Add(entity);
            if (await _uow.SaveAsync() <= -1)
            {
                return ServiceResponse<DocumentStatusDto>.Return500();
            }
            return ServiceResponse<DocumentStatusDto>.ReturnResultWith201(_mapper.Map<DocumentStatusDto>(entity));
        }
    }
}
