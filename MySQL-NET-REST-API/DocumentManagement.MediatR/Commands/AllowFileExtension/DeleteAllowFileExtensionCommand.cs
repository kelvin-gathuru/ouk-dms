using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteAllowFileExtensionCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
