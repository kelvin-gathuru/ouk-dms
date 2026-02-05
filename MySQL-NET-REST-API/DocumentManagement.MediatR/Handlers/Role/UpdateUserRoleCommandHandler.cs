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

public class UpdateUserRoleCommandHandler(
     IHubContext<UserHub, IHubClient> _hubContext,
     IConnectionMappingRepository _connectionMappingRepository,
     IUserRepository _userRepository,
     IUserRoleRepository _userRoleRepository,
     IUnitOfWork<DocumentContext> _uow,
     IMapper _mapper
) : IRequestHandler<UpdateUserRoleCommand, UserRoleDto>
{

    public async Task<UserRoleDto> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRoles = await _userRoleRepository.All.Where(c => c.RoleId == request.Id).ToListAsync();
        var userRolesToAdd = request.UserRoles.Where(c => !userRoles.Select(c => c.UserId).Contains(c.UserId.Value)).ToList();
        _userRoleRepository.AddRange(_mapper.Map<List<UserRole>>(userRolesToAdd));
        var userRolesToDelete = userRoles.Where(c => !request.UserRoles.Select(cs => cs.UserId).Contains(c.UserId)).ToList();
        _userRoleRepository.RemoveRange(userRolesToDelete);

        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new UserRoleDto
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
        return new UserRoleDto();
    }
}
