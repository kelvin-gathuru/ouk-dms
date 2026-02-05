using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class CheckDocumentPermissionCommand : IRequest<ServiceResponse<bool>>
{
    public Guid DocumentId { get; set; }
}
