using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRoleRepository _userRoleRepository;
    readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    public UpdateUserCommandHandler(
        IUserRoleRepository userRoleRepository,
        IMapper mapper,
        IUnitOfWork<DocumentContext> uow,
        UserManager<User> userManager
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _userRoleRepository = userRoleRepository;
        _uow = uow;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByIdAsync(request.Id.ToString());
        if (appUser == null)
        {
            var errorDto = new UserDto
            {
                StatusCode = 409,
                Messages = new List<string> { "User does not exist." }
            };
            return errorDto;
        }
        appUser.FirstName = request.FirstName;
        appUser.LastName = request.LastName;
        appUser.PhoneNumber = request.PhoneNumber;
        appUser.IsSuperAdmin = request.IsSuperAdmin;
        IdentityResult result = await _userManager.UpdateAsync(appUser);

        // Update User Role
        var userRoles = _userRoleRepository.All.Where(c => c.UserId == appUser.Id).ToList();
        var rolesToAdd = request.UserRoles.Where(c => !userRoles.Select(c => c.RoleId).Contains(c.RoleId)).ToList();
        _userRoleRepository.AddRange(_mapper.Map<List<UserRole>>(rolesToAdd));
        var rolesToDelete = userRoles.Where(c => !request.UserRoles.Select(cs => cs.RoleId).Contains(c.RoleId)).ToList();
        _userRoleRepository.RemoveRange(rolesToDelete);
        if (await _uow.SaveAsync() <= -1 && !result.Succeeded)
        {
            var errorDto = new UserDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        return _mapper.Map<UserDto>(appUser);
    }
}
