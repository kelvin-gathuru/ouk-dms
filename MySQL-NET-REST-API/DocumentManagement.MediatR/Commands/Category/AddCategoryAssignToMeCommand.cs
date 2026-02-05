using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;


namespace DocumentManagement.MediatR.Commands
{
    public class AddCategoryAssignToMeCommand : IRequest<CategoryDto>
    {
        [Required(ErrorMessage = "Category Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
    }
}
