using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class UserLoginCommand : IRequest<UserAuthDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteIp { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
