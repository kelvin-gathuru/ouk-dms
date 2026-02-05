using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class CompanyProfileRepository : GenericRepository<CompanyProfile, DocumentContext>, ICompanyProfileRepository
    {
        public CompanyProfileRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

