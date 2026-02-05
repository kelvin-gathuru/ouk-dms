using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class GetCategoryPermissionQuery : IRequest<List<CategoryPermissionDto>>
    {
        public Guid CategoryId { get; set; }
    }
}
