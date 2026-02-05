using Microsoft.AspNetCore.Identity;
using System;

namespace DocumentManagement.Data
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; }
    }
}
