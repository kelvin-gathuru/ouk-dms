using System;
using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Queries;
public class GetSharePermissionQuery : IRequest<SharePermissionDto>
{
    public Guid DocumentId { get; set; }
    public Guid CategoryId { get; set; }
}
