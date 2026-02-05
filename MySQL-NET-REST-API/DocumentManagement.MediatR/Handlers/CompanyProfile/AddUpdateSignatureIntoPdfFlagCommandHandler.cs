using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class AddUpdateSignatureIntoPdfFlagCommandHandler(ICompanyProfileRepository companyProfileRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddUpdateSignatureIntoPdfFlagCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(AddUpdateSignatureIntoPdfFlagCommand request, CancellationToken cancellationToken)
    {
        var companyProfile = await companyProfileRepository
            .FindBy(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (companyProfile == null)
        {
            return ServiceResponse<bool>.Return404("No record found.");
        }

        companyProfile.AllowPdfSignature = request.AllowSignatureIntoPdf;
        companyProfileRepository.Update(companyProfile);

        if (await _uow.SaveAsync() <= -1)
        {
            return ServiceResponse<bool>.Return500();
        }

        return ServiceResponse<bool>.ReturnResultWith200(true);
    }
}

