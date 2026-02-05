using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetPageHelperByCodeCommand : IRequest<ServiceResponse<PageHelperDto>>
    {
        public string Code { get; set; }
    }
}
