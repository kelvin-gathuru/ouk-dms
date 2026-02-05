using DocumentManagement.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
        public Guid? PageActionId { get; set; }
        [ForeignKey("PageActionId")]
        public PageAction PageAction { get; set; }
    }
}
