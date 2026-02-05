using System.Collections.Generic;
using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllAssignToMeCategoryQuery : IRequest<List<CategoryDto>>
    {
        public bool IsParentOnly { get; set; }
    }
}
