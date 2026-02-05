using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository;
public class AIPromptTemplateRepository : GenericRepository<AIPromptTemplate, DocumentContext>, IAIPromptTemplateRepository
{
    public AIPromptTemplateRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
    {
    }
}
