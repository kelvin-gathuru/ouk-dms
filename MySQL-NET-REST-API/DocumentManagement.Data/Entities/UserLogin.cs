using Microsoft.AspNetCore.Identity;
using System;

namespace DocumentManagement.Data
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}
