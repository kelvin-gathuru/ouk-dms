using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetCategoryQuery : IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
    }
}
