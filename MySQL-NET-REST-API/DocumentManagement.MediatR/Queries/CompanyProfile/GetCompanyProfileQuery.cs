using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetCompanyProfileQuery: IRequest<ServiceResponse<CompanyProfileDto>>
    {
    }
}
