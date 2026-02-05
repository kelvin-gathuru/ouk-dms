using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class AddAllowFileExtensionCommand : IRequest<ServiceResponse<AllowFileExtensionDto>>
    {
        public FileType FileType { get; set; }
        public string Extension { get; set; }  
    }
}
