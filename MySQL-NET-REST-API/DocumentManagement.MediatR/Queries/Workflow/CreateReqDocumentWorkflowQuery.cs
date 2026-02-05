using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class CreateReqDocumentWorkflowQuery : IRequest<ServiceResponse<bool>>
    {
        public Guid WorkflowId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid StorageSettingId { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
    }
}
