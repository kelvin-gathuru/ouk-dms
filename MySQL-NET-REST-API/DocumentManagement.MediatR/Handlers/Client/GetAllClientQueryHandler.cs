using System.Collections.Generic;
using System.Linq;
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
    public class GetAllClientQueryHandler(IClientRepository _clientRepository, IMapper _mapper) : IRequestHandler<GetAllClientQuery, ServiceResponse<List<ClientDto>>>
    {
        public async Task<ServiceResponse<List<ClientDto>>> Handle(GetAllClientQuery request, CancellationToken cancellationToken)
        {
            var entities = await _clientRepository.All
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync(cancellationToken);
            if (entities.Count == 0)
            {
                return ServiceResponse<List<ClientDto>>.ReturnResultWith200([]);
            }
            var entityDtos = _mapper.Map<List<ClientDto>>(entities);
            return ServiceResponse<List<ClientDto>>.ReturnResultWith200(entityDtos);
        }
    }
}