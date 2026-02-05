using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateAllowFileExtensionCommand : IRequest<ServiceResponse<AllowFileExtensionDto>>
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string Extension { get; set; }
    }
}
