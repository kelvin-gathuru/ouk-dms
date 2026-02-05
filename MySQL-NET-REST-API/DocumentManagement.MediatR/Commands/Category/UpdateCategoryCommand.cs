using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateCategoryCommand : IRequest<CategoryDto>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
    }
}
