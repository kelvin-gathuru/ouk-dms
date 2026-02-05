using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class GetDocumentSummaryCommand : IRequest<ServiceResponse<string>>
{
    public Guid DocumentId { get; set; }
}
