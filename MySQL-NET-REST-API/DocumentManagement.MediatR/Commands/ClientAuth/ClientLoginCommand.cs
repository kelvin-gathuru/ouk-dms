using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    using DocumentManagement.Data.Dto;

    public class ClientLoginCommand : IRequest<ServiceResponse<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
