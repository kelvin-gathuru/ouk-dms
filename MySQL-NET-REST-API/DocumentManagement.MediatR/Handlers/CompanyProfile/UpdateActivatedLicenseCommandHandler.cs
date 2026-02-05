using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class UpdateActivatedLicenseCommandHandler(ICompanyProfileRepository companyProfileRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<UpdateActivatedLicenseCommand, bool>
{
    public async Task<bool> Handle(UpdateActivatedLicenseCommand request, CancellationToken cancellationToken)
    {
        var companyProfile = await companyProfileRepository.All.FirstOrDefaultAsync();
        if (companyProfile == null)
        {
            return false; // Company profile not found
        }
        companyProfile.PurchaseCode = request.PurchaseCode;
        companyProfile.LicenseKey = request.LicenseKey;
        companyProfileRepository.Update(companyProfile);
        if (await _uow.SaveAsync() <= -1)
        {
            return false;
        }
        return true;
    }
}
