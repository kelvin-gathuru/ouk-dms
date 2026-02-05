using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class GetDocumentDetailById : IRequest<DocumentDto>
{
    public Guid Id { get; set; }
}
