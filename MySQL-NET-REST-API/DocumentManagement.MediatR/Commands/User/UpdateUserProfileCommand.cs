using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateUserProfileCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
