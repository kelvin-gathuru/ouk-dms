using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserAuthDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILoginAuditRepository _loginAuditRepository;
        private readonly IHubContext<UserHub, IHubClient> _hubContext;

        public UserLoginCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<User> userManager,
            ILoginAuditRepository loginAuditRepository,
            IHubContext<UserHub, IHubClient> hubContext
            )
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _loginAuditRepository = loginAuditRepository;
            _hubContext = hubContext;
        }
        public async Task<UserAuthDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var loginAudit = new LoginAuditDto
            {
                UserName = request.UserName,
                RemoteIP = request.RemoteIp,
                Status = "Success",
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                loginAudit.Status = "Error";
                await _loginAuditRepository.LoginAudit(loginAudit);
                return new UserAuthDto
                {
                    StatusCode = 401,
                    Messages = new List<string> { "UserName Or Password is InCorrect." }
                };
            }

            await _loginAuditRepository.LoginAudit(loginAudit);
            var authUser = await _userRepository.BuildUserAuthObject(user);
            var onlineUser = new SignlarUser
            {
                Email = authUser.Email,
                Id = authUser.Id
            };
            await _hubContext.Clients.All.Joined(onlineUser);
            return authUser;

        }
    }
}
