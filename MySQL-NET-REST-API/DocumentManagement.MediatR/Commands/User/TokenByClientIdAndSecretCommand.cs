

using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public record TokenByClientIdAndSecretCommand : IRequest<UserAuthDto>
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
