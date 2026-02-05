using System;
using System.Threading;
using System.Threading.Tasks;
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

namespace DocumentManagement.MediatR.Handlers
{
    public class AddClientCommandHandler(IClientRepository _clientRepository, IMapper _mapper, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddClientCommand, ServiceResponse<ClientDto>>
    {
        public async Task<ServiceResponse<ClientDto>> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Client>(request);
            entity.Id = Guid.NewGuid();
            _clientRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<ClientDto>.Return500();
            }
            var entityDto = _mapper.Map<ClientDto>(entity);
            return ServiceResponse<ClientDto>.ReturnResultWith201(entityDto);
        }
    }
}