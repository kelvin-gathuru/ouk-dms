using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;

public class UserDto : ErrorStatusCode
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSuperAdmin { get; set; }

    public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
    public List<UserClaimDto> UserClaims { get; set; } = new List<UserClaimDto>();

}
