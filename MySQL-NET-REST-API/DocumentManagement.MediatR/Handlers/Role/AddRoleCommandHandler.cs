using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly IRoleClaimRepository _roleClaimRepository;
        public AddRoleCommandHandler(
           IRoleRepository roleRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            IRoleClaimRepository roleClaimRepository
            )
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _uow = uow;
            _roleClaimRepository = roleClaimRepository;
        }
        public async Task<RoleDto> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _roleRepository
                .FindByInclude(v => v.Name == request.Name, c => c.RoleClaims)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();

            if (entityExist != null && !entityExist.IsDeleted)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Role Name already exist." }
                };
                return errorDto;
            }

            // Update Role
            if (entityExist != null)
            {
                entityExist.Name = request.Name;
                entityExist.NormalizedName = request.Name;
                entityExist.IsDeleted = false;
                _roleRepository.Update(entityExist);

                // update Role Claim
                var roleClaimsToDelete = entityExist.RoleClaims.ToList();
                _roleClaimRepository.RemoveRange(roleClaimsToDelete);

                request.RoleClaims.ForEach(claim =>
                {
                    claim.ClaimType = claim.ClaimType.Replace(" ", "_");
                    claim.RoleId = entityExist.Id;
                });
                _roleClaimRepository.AddRange(_mapper.Map<List<RoleClaim>>(request.RoleClaims));
            }
            else
            {
                // add new role.
                entityExist = _mapper.Map<Role>(request);
                entityExist.Id = Guid.NewGuid();
                entityExist.NormalizedName = entityExist.Name;
                entityExist.RoleClaims.ToList().ForEach(claim => claim.ClaimType = claim.ClaimType.Replace(" ", "_"));
                _roleRepository.Add(entityExist);
            }

            if (await _uow.SaveAsync() <= -1)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var entityDto = _mapper.Map<RoleDto>(entityExist);
            return entityDto;
        }
    }
}
