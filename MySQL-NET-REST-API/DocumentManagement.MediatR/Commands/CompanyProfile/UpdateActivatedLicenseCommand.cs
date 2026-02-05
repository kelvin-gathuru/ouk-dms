using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class UpdateActivatedLicenseCommand : IRequest<bool>
{
    public string PurchaseCode { get; set; }
    public string LicenseKey { get; set; }
}
