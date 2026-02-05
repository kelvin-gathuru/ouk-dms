using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class RefreshTokenCommandHandler(
    IUserRepository _userRepository,
    UserManager<User> _userManager,
    IHubContext<UserHub, IHubClient> _hubContext,
     UserInfoToken _userInfo
    ) : IRequestHandler<RefreshTokenCommand, UserAuthDto>
{
    public async Task<UserAuthDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _userInfo.Id;
        var user = await _userManager.FindByIdAsync(userId.ToString());
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
