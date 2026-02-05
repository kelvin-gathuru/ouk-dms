using System;

namespace DocumentManagement.Data.Dto
{
    public class RoleClaimDto : ErrorStatusCode
    {
        public int Id { get; set; }
        public Guid RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Guid PageActionId { get; set; }
    }
}
