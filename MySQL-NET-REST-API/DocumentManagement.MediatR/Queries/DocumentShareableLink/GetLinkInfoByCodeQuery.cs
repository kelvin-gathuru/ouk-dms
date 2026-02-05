using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetLinkInfoByCodeQuery : IRequest<ServiceResponse<DocumentShareableLinkDto>>
    {
        public string Code { get; set; }
    }
}
