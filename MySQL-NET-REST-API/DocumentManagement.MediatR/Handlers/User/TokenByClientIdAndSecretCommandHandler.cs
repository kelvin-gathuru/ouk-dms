using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public record TokenByClientIdAndSecretCommandHandler(
      IUserRepository userRepository
            ) : IRequestHandler<TokenByClientIdAndSecretCommand, UserAuthDto>
{
    public async Task<UserAuthDto> Handle(TokenByClientIdAndSecretCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.All.Where(x => x.ClientId == request.ClientId && x.ClientSecretHash == request.ClientSecret).FirstOrDefault();
        if (user == null)
        {
            return new UserAuthDto
            {
                StatusCode = 401,
                Messages = new List<string> { "ClientId or ClientSecret is InCorrect." }
            };
        }
        var authUser = await userRepository.BuildUserAuthObject(user);
        return authUser;
    }
}

