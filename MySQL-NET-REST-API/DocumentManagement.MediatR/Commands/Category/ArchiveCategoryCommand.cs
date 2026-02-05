using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class ArchiveCategoryCommand : IRequest<ServiceResponse<CategoryDto>>
{
    public Guid CategoryId { get; set; }
}