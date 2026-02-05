using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Queries
{
    public class CancelWorkflowInstanceQuery : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}