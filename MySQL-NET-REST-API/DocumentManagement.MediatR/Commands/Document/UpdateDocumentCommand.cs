using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands;

public class UpdateDocumentCommand : IRequest<DocumentDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? DocumentStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public List<DocumentMetaDataDto> DocumentMetaDatas { get; set; } = new List<DocumentMetaDataDto>();
    public int? RetentionPeriodInDays { get; set; }
    public RETENTION_ACTION_ENUM? OnExpiryAction { get; set; }
}
