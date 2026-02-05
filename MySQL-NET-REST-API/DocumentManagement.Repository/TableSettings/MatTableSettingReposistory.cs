using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;


namespace DocumentManagement.Repository
{
    public class MatTableSettingReposistory : GenericRepository<MatTableSetting, DocumentContext>,
           IMatTableSettingReposistory
    {
        public MatTableSettingReposistory(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {

        }
    }
}
