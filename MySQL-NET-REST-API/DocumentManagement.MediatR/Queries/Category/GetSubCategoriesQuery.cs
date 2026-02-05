using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetSubCategoriesQuery : IRequest<List<CategoryDto>>
    {
        public Guid Id { get; set; }
    }
}
