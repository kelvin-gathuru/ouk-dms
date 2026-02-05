using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class AddDocumentSignatureWithPositionCommand : IRequest<ServiceResponse<DocumentSignatureDto>>
{
    public Guid DocumentId { get; set; }
    public string SignatureUrl { get; set; }
    public int PageNumber { get; set; }
    public float YAxis { get; set; }
    public float XAxis { get; set; }
    public string UserName { get; set; }
    public float ViewportWidth { get; set; }
    public float ViewportHeight { get; set; }
    public string Password { get; set; } = string.Empty;
}
