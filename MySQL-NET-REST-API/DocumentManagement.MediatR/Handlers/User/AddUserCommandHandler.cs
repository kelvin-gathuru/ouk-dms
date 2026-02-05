using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    public AddUserCommandHandler(
        IMapper mapper,
        UserManager<User> userManager,
        IUserRepository userRepository
,
        IUnitOfWork<DocumentContext> uow)
    {
        _mapper = mapper;
        _userManager = userManager;
        _userRepository = userRepository;
        _uow = uow;
    }
    public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var allUser = await _userRepository.All.IgnoreQueryFilters().ToListAsync();
        var appUser = await _userRepository.All.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Email.ToLower() == request.Email.ToLower());
        if (appUser != null && appUser.IsDeleted)
        {
            var currentDateTime = $"{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}{DateTime.UtcNow.Day}{DateTime.UtcNow.Hour}{DateTime.UtcNow.Minute}{DateTime.UtcNow.Second}{DateTime.UtcNow.Millisecond}";
            appUser.UserName = $"{appUser.UserName}_IsDeleted_{currentDateTime}";
            var email = appUser.Email.Split('@');

            appUser.Email = $"{email[0]}_IsDeleted_{currentDateTime}{email[1]}";
            appUser.NormalizedUserName = appUser.UserName.ToUpper();
            appUser.NormalizedEmail = appUser.Email.ToUpper();
            _userRepository.Update(appUser);
        }
        if (appUser != null && !appUser.IsDeleted)
        {
            var errorDto = new UserDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Email already exist for another user." }
            };
            return errorDto;
        }
        var entity = _mapper.Map<User>(request);
        entity.Id = Guid.NewGuid();
        entity.ClientId = Guid.NewGuid().ToString();
        entity.ClientSecretHash = ClientSecretGenerator.GenerateClientSecret();

        IdentityResult result = await _userManager.CreateAsync(entity);
        if (await _uow.SaveAsync() <= -1 && !result.Succeeded)
        {
            var errorDto = new UserDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        if (!string.IsNullOrEmpty(request.Password))
        {

            string code = await _userManager.GeneratePasswordResetTokenAsync(entity);
            IdentityResult passwordResult = await _userManager.ResetPasswordAsync(entity, code, request.Password);
            if (!passwordResult.Succeeded)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
        }

        return _mapper.Map<UserDto>(entity);
    }
}
