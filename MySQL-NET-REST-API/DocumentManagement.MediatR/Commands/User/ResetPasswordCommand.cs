using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class ResetPasswordCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
