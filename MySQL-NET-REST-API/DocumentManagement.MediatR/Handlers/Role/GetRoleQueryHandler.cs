using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleDto>
    {

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoleQueryHandler> _logger;

        public GetRoleQueryHandler(
           IRoleRepository roleRepository,
            IMapper mapper,
            ILogger<GetRoleQueryHandler> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<RoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var entity =await _roleRepository.AllIncluding(c => c.UserRoles, cs => cs.RoleClaims)
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync();
            if (entity != null)
                return _mapper.Map<RoleDto>(entity);
            else
            {
                var entityDto = new RoleDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found Role" }
                };
                _logger.LogWarning($"Not Found Role {entityDto.Name}", entityDto);
                return entityDto;
            }
        }
    }
}
