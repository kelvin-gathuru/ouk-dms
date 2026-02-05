using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateCompanyProfileCommandHandler(IMapper _mapper, ICompanyProfileRepository _compnayProfileRepository, IUnitOfWork<DocumentContext> _uow, IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _hostingEnvironment) : IRequestHandler<UpdateCompanyProfileCommand, ServiceResponse<CompanyProfileDto>>
{
    public async Task<ServiceResponse<CompanyProfileDto>> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await _compnayProfileRepository
            .FindBy(c => c.Name == request.Name && c.Id != request.Id)
            .FirstOrDefaultAsync();

        if (nameExists != null)
        {
            return ServiceResponse<CompanyProfileDto>.Return409("Company Profile Name already exists.");
        }

        var entity = await _compnayProfileRepository
            .FindBy(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return ServiceResponse<CompanyProfileDto>.Return404("No record found.");
        }
        var requestContext = _httpContextAccessor.HttpContext.Request;
        var imagesDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }
        if (request.LogoFile != null && request.LogoFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(entity.LogoUrl) && File.Exists(Path.Combine(imagesDirectory, entity.LogoUrl)))
            {
                File.Delete(Path.Combine(imagesDirectory, entity.LogoUrl));
            }

            var newLogoFileName = Guid.NewGuid() + Path.GetExtension(request.LogoFile.FileName);
            var logoFilePath = Path.Combine(imagesDirectory, newLogoFileName);
            using (var stream = new FileStream(logoFilePath, FileMode.Create))
            {
                await request.LogoFile.CopyToAsync(stream);
            }

            entity.LogoUrl = newLogoFileName;
        }

        if (request.BannerFile != null && request.BannerFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(entity.BannerUrl) && File.Exists(Path.Combine(imagesDirectory, entity.BannerUrl)))
            {
                File.Delete(Path.Combine(imagesDirectory, entity.BannerUrl));
            }

            var newBannerFileName = Guid.NewGuid() + Path.GetExtension(request.BannerFile.FileName);
            var bannerFilePath = Path.Combine(imagesDirectory, newBannerFileName);
            using (var stream = new FileStream(bannerFilePath, FileMode.Create))
            {
                await request.BannerFile.CopyToAsync(stream);
            }

            entity.BannerUrl = newBannerFileName;
        }

        if (request.LogoIconFile != null && request.LogoIconFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(entity.LogoIconUrl) && File.Exists(Path.Combine(imagesDirectory, entity.LogoIconUrl)))
            {
                File.Delete(Path.Combine(imagesDirectory, entity.LogoIconUrl));
            }

            var newLogoIconFileName = Guid.NewGuid() + Path.GetExtension(request.LogoIconFile.FileName);
            var logoIconFilePath = Path.Combine(imagesDirectory, newLogoIconFileName);
            using (var stream = new FileStream(logoIconFilePath, FileMode.Create))
            {
                await request.LogoIconFile.CopyToAsync(stream);
            }

            entity.LogoIconUrl = newLogoIconFileName;
        }

        entity.Name = request.Name;
        _compnayProfileRepository.Update(entity);

        if (await _uow.SaveAsync() <= -1)
        {
            return ServiceResponse<CompanyProfileDto>.Return500();
        }
        entity.LogoUrl = string.IsNullOrEmpty(entity.LogoUrl) ? null : $"{requestContext.Scheme}://{requestContext.Host}/images/{entity.LogoUrl}";
        entity.BannerUrl = string.IsNullOrEmpty(entity.BannerUrl) ? null : $"{requestContext.Scheme}://{requestContext.Host}/images/{entity.BannerUrl}";
        entity.LogoIconUrl = string.IsNullOrEmpty(entity.LogoIconUrl) ? null : $"{requestContext.Scheme}://{requestContext.Host}/images/{entity.LogoIconUrl}";
        return ServiceResponse<CompanyProfileDto>.ReturnResultWith201(_mapper.Map<CompanyProfileDto>(entity));
    }
}
