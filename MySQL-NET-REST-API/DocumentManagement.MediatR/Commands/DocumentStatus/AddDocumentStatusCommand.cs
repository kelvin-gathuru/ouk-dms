using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentStatusCommand: IRequest<ServiceResponse<DocumentStatusDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }
    }
}
