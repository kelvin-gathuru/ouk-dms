using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.MediatR.Commands;

public class UpdateCompanyProfileCommand : IRequest<ServiceResponse<CompanyProfileDto>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IFormFile LogoFile { get; set; }
    public IFormFile BannerFile { get; set; }
    public IFormFile LogoIconFile { get; set; }
}
