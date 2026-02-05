using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands;
public class GetAllDescendantsUsingCTEByParentIdCommand : IRequest<ServiceResponse<List<CategoryDto>>>
{
    public Guid ParentId { get; set; }
}
