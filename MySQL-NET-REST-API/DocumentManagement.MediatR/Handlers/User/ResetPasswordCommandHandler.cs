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
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public ResetPasswordCommandHandler(
            IMapper mapper,
            UserManager<User> userManager
            )
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByEmailAsync(request.UserName);
            if (entity == null)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "User not Found." }
                };
                return errorDto;
            }
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
            var entityDto = new UserDto
            {
                StatusCode = 200,
                Messages = new List<string> { "New password set successfully." }
            };
            return entityDto;
        }
    }
}
