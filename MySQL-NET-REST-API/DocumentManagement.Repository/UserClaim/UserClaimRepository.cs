using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class UserClaimRepository : GenericRepository<UserClaim, DocumentContext>,
           IUserClaimRepository
    {
        public UserClaimRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}