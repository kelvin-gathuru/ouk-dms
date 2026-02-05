using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class AddDocumentChunkWindowSharedCommand : IRequest<ServiceResponse<DocumentDto>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public Guid CategoryId { get; set; }
    public string Extension { get; set; }
    public StorageType StorageType { get; set; }
    public string DocumentMetaDataString { get; set; }
    public string DocumentUserPermissionString { get; set; }
    public string DocumentRolePermissionString { get; set; }
    public Guid StorageSettingId { get; set; }
    public Guid? DocumentStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public bool IsAssignToMe { get; set; } = false;
}

