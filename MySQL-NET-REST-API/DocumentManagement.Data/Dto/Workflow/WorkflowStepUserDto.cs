using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowStepUserDto
    {
        public Guid? WorkflowStepId { get; set; }
        public Guid? UserId { get; set; }
        public WorkflowStepDto WorkflowStep { get; set; }
        public UserDto User { get; set; }
    }
}
