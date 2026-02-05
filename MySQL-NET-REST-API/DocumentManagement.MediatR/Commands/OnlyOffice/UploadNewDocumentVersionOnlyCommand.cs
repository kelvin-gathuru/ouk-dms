using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class UploadNewDocumentVersionOnlyCommand : IRequest<ServiceResponse<DocumentVersionDto>>
{
    public Guid DocumentId { get; set; }
    public byte[] File { get; set; }
}

