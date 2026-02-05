using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllCategoryQuery : IRequest<List<CategoryDto>>
    {
        public bool IsParentOnly { get; set; }
    }
}
