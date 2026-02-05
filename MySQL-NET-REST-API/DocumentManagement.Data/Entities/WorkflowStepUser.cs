using System;

namespace DocumentManagement.Data
{
    public class WorkflowStepUser
    {
        public Guid WorkflowStepId { get; set; }
        public Guid UserId { get; set; }
        public WorkflowStep WorkflowStep { get; set; }
        public User User { get; set; }

    }
}
