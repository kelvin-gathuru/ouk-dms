using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class CreatePageHelperCommand : IRequest<ServiceResponse<PageHelperDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
