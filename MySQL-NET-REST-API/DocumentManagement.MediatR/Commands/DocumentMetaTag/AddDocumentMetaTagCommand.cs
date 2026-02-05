using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class AddDocumentMetaTagCommand : IRequest<ServiceResponse<DocumentMetaTagDto>>
{
    public MetaTagType Type { get; set; }
    public string Name { get; set; }
    public bool IsEditable { get; set; } = true;
}
