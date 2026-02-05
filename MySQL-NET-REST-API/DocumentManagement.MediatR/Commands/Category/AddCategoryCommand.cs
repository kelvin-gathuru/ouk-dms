using System;
using System.ComponentModel.DataAnnotations;
using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands;

public class AddCategoryCommand : IRequest<CategoryDto>
{
    [Required(ErrorMessage = "Folder Name is required")]
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? ParentId { get; set; }
}
