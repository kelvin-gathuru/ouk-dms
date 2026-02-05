using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class GetDocumentMetaTagCommand : IRequest<ServiceResponse<DocumentMetaTagDto>>
{
    public Guid Id { get; set; }
}