using DocumentManagement.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSuperAdmin { get; set; } = false;
    public string ClientId { get; set; }
    public string ClientSecretHash { get; set; }
    public bool IsSystemUser { get; set; } = false;
    public virtual ICollection<UserClaim> UserClaims { get; set; }
    public virtual ICollection<UserLogin> UserLogins { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<DocumentUserPermission> DocumentUserPermissions { get; set; }
    public virtual ICollection<UserNotification> UserNotifications { get; set; }
    public virtual ICollection<CategoryUserPermission> CategoryUserPermissions { get; set; }
}
