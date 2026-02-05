using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;


namespace DocumentManagement.MediatR.Commands;

public class UploadNewDocumentChunkVersionCommand : IRequest<ServiceResponse<DocumentVersionDto>>
{
    public Guid DocumentId { get; set; }
    public string Extension { get; set; }
    public string Url { get; set; }
    public string Comment { get; set; }
    public bool isSignatureExists { get; set; }

}
