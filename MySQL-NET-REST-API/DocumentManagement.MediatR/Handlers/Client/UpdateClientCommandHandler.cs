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
    public class UpdateClientCommandHandler(IClientRepository _clientRepository, IUnitOfWork<DocumentContext> _uow, IMapper _mapper) : IRequestHandler<UpdateClientCommand, ServiceResponse<ClientDto>>
    {
        public async Task<ServiceResponse<ClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _clientRepository.All
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (entityExist == null)
            {
                return ServiceResponse<ClientDto>.Return409("Client does not exists.");
            }
            var entity = _mapper.Map<Client>(request);
            _clientRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<ClientDto>.Return500();
            }
            var entityDto = _mapper.Map<ClientDto>(entity);
            return ServiceResponse<ClientDto>.ReturnResultWith200(entityDto);
        }
    }
}
