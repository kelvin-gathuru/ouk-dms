using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteCategoryCommand: IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
    }
}
