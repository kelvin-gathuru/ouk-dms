using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Data.Dto
{
    public class PendingTransition
    {
        [Key]
        public Guid TransitionId { get; set; }
        public Guid FromStepId { get; set; }
        public Guid ToStepId { get; set; }
        public string TransitionName { get; set; }
    }
}
