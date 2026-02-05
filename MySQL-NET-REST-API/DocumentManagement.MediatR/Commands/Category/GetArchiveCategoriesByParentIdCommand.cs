using System;
using System.Collections.Generic;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class GetArchiveCategoriesByParentIdCommand : IRequest<ServiceResponse<List<CategoryDto>>>
{
    public Guid? ParentId { get; set; }
}
