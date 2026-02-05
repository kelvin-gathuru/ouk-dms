using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentAuditTrailCommand : IRequest<DocumentAuditTrailDto>
    {
        public Guid DocumentId { get; set; }
        public string OperationName { get; set; }
    }
}
