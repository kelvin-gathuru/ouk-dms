using System.Collections.Generic;
using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands;

public class AddUserCommand : IRequest<UserDto>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();

}
