using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateRoleCommandHandler(
        IHubContext<UserHub, IHubClient> _hubContext,
        IConnectionMappingRepository _connectionMappingRepository,
        IUserRepository _userRepository,
        IRoleRepository _roleRepository,
        IRoleClaimRepository _roleClaimRepository,
        IUnitOfWork<DocumentContext> _uow,
        IMapper _mapper) : IRequestHandler<UpdateRoleCommand, RoleDto>
{


    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _roleRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
             .FirstOrDefaultAsync();
        if (entityExist != null)
        {
            var errorDto = new RoleDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Role Name Already Exist." }
            };
            return errorDto;
        }

        // Update Role
        var entity = _mapper.Map<Role>(request);
        entityExist = await _roleRepository.FindByInclude(v => v.Id == request.Id, c => c.RoleClaims).FirstOrDefaultAsync();
        entityExist.Name = entity.Name;
        entityExist.NormalizedName = entity.Name;
        _roleRepository.Update(entityExist);

        // update Role Claim
        var roleClaims = entityExist.RoleClaims.ToList();
        var roleClaimsToAdd = request.RoleClaims.Where(c => !roleClaims.Select(c => c.Id).Contains(c.Id)).ToList();
        roleClaimsToAdd.ForEach(claim => claim.ClaimType = claim.ClaimType.Replace(" ", "_"));
        _roleClaimRepository.AddRange(_mapper.Map<List<RoleClaim>>(roleClaimsToAdd));
        var roleClaimsToDelete = roleClaims.Where(c => !request.RoleClaims.Select(cs => cs.Id).Contains(c.Id)).ToList();
        _roleClaimRepository.RemoveRange(roleClaimsToDelete);

        // TODO: update user Role
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new RoleDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var users = await _userRepository.GetUsersByRoleId(request.Id);
        if (users.Count > 0)
        {
            foreach (var user in users)
            {
                var currentUser = _connectionMappingRepository.GetUserInfoById(user);
                if (currentUser != null)
                {
                    await _hubContext.Clients.Client(currentUser.ConnectionId).UpdateUserPermission(Guid.Parse(currentUser.Id));
                }
            }
        }
        var entityDto = _mapper.Map<RoleDto>(entity);
        return entityDto;
    }
}
