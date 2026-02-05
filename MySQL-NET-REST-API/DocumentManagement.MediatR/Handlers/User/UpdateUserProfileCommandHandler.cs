using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private UserInfoToken _userInfoToken;
        public UpdateUserProfileCommandHandler(
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            UserInfoToken userInfoToken,
            UserManager<User> userManager
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _uow = uow;
            _userInfoToken = userInfoToken;
        }

        public async Task<UserDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByIdAsync(_userInfoToken.Id.ToString());
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
            IdentityResult result = await _userManager.UpdateAsync(appUser);
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

}
