using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class ClientActivationCommand : IRequest<ServiceResponse<bool>>
    {
        public string Email { get; set; }
        public string ActivationCode { get; set; }
    }
}
