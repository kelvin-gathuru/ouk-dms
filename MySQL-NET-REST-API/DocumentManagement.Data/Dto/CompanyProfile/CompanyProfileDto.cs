using System;

namespace DocumentManagement.Data.Dto;

public class CompanyProfileDto : ErrorStatusCode
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public bool AllowSignatureIntoPdf { get; set; }
    public string LogoIconUrl { get; set; }
    public string LicenseKey { get; set; } = string.Empty;
    public string PurchaseCode { get; set; } = string.Empty;
}
