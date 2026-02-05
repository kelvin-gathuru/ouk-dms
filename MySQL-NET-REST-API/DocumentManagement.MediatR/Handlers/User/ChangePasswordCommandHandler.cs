using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        public ChangePasswordCommandHandler(
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<UserDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.OldPassword, false, false);
            if (!result.Succeeded)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 422,
                    Messages = new List<string> { "Old Password does not match." }
                };
                return errorDto;
            }

            var entity = await _userManager.FindByNameAsync(request.UserName);
            string code = await _userManager.GeneratePasswordResetTokenAsync(entity);
            IdentityResult passwordResult = await _userManager.ResetPasswordAsync(entity, code, request.NewPassword);
            if (!passwordResult.Succeeded)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var successDto = new UserDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Password Changed successfully." }
            };
            return successDto;
           
        }
    }
}
