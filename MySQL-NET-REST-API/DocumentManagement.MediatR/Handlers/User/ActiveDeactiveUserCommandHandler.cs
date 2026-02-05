using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ActiveDeactiveUserCommandHandler : IRequestHandler<ActiveDeactiveUserCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ActiveDeactiveUserCommandHandler(UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(ActiveDeactiveUserCommand request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (appUser == null)
            {
                var errorDto = new UserDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "User does not exist." }
                };
                return errorDto;
            }

            appUser.IsDeleted = !appUser.IsDeleted;

            IdentityResult result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
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
}
