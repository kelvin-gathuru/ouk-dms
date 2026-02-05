using DocumentManagement.Data.Dto;
using MediatR;
using System;


namespace DocumentManagement.MediatR.Commands
{
    public class DeleteCategoryUserPermissionCommand : IRequest<CategoryUserPermissionDto>
    {
        public Guid Id { get; set; }
    }
}
