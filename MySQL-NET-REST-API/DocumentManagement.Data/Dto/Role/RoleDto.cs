using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class RoleDto : ErrorStatusCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RoleClaimDto> RoleClaims { get; set; }

    }
}
