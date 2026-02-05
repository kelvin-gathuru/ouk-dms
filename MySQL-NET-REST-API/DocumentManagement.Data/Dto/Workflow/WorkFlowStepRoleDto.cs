using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowStepRoleDto
    {
        public Guid? WorkflowStepId { get; set; }
        public Guid? RoleId { get; set; }
        public WorkflowStepDto WorkflowStep { get; set; }
        public RoleDto Role { get; set; }
    }
}
