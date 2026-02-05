using System;

namespace DocumentManagement.Data
{
    public class WorkflowStepRole
    {
        public Guid WorkflowStepId { get; set; }
        public Guid RoleId { get; set; }
        public WorkflowStep WorkflowStep { get; set; }
        public Role Role { get; set; }

    }
}
