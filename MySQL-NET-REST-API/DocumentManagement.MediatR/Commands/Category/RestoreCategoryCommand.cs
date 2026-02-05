using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class RestoreCategoryCommand : IRequest<ServiceResponse<CategoryDto>>
{
    public Guid CategoryId { get; set; }
}