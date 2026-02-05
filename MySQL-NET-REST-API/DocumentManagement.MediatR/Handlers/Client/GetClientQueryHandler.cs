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
    public class GetClientQueryHandler(IClientRepository _clientRepository,IMapper _mapper) : IRequestHandler<GetClientQuery, ServiceResponse<ClientDto>>
    {
        public async Task<ServiceResponse<ClientDto>> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            var entity = await _clientRepository.All
               .FirstOrDefaultAsync(w => w.Id == request.Id);
            if (entity == null)
            {
                return ServiceResponse<ClientDto>.Return409("Not found");
            }
            var entityDto = _mapper.Map<ClientDto>(entity);
            return ServiceResponse<ClientDto>.ReturnResultWith200(entityDto);
        }
    }
}
