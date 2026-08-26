using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GoogleLoginCommand : IRequest<UserAuthDto>
    {
        public string IdToken { get; set; }
    }
}
