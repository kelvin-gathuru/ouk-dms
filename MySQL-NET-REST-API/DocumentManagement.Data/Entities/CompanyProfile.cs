using System;

namespace DocumentManagement.Data.Entities;

public class CompanyProfile : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public bool AllowPdfSignature { get; set; }
    public string OpenAIAPIKey { get; set; }
    public string LogoIconUrl { get; set; }
    public string LicenseKey { get; set; } = string.Empty;
    public string PurchaseCode { get; set; } = string.Empty;
    public string GeminiAPIKey { get; set; }
}
