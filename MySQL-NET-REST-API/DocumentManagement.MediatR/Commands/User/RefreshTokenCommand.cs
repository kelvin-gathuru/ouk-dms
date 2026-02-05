using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class RefreshTokenCommand : IRequest<UserAuthDto>
{
}
