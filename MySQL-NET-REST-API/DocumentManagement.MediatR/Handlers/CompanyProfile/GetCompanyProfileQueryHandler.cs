using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetCompanyProfileQueryHandler(ICompanyProfileRepository _compnayProfileRepository, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<GetCompanyProfileQuery, ServiceResponse<CompanyProfileDto>>
{
    public async Task<ServiceResponse<CompanyProfileDto>> Handle(GetCompanyProfileQuery request, CancellationToken cancellationToken)
    {
        var entity = await _compnayProfileRepository.All.FirstOrDefaultAsync();

        if (entity == null)
        {
            return ServiceResponse<CompanyProfileDto>.Return404("Not found");
        }

        var requestContext = _httpContextAccessor.HttpContext.Request;

        var companyProfile = new CompanyProfileDto
        {
            Id = entity.Id,
            Name = entity.Name,
            LogoUrl = string.IsNullOrEmpty(entity.LogoUrl) ? null : $"{requestContext.Scheme}://{requestContext.Host}/images/{entity.LogoUrl}",
            BannerUrl = string.IsNullOrEmpty(entity.BannerUrl) ? null : $"{requestContext.Scheme}://{requestContext.Host}/images/{entity.BannerUrl}",
            AllowSignatureIntoPdf = entity.AllowPdfSignature,
            LogoIconUrl = string.IsNullOrEmpty(entity.LogoIconUrl) ? null : $"{requestContext.Scheme}://{requestContext.Host}/images/{entity.LogoIconUrl}",
            PurchaseCode = entity.PurchaseCode,
            LicenseKey = entity.LicenseKey
        };

        return ServiceResponse<CompanyProfileDto>.ReturnResultWith200(companyProfile);
    }
}
