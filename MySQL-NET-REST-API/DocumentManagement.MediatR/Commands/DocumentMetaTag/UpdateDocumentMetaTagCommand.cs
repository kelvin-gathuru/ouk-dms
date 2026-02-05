using System;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class UpdateDocumentMetaTagCommand : IRequest<ServiceResponse<DocumentMetaTagDto>>
{
    public Guid Id { get; set; }
    public MetaTagType Type { get; set; }
    public string Name { get; set; }
    public bool IsEditable { get; set; }
}
