using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class ClientForgotPasswordCommand : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
    }
}
