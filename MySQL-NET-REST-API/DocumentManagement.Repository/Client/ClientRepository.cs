using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class ClientRepository : GenericRepository<Client, DocumentContext>, IClientRepository
    {
        public ClientRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}