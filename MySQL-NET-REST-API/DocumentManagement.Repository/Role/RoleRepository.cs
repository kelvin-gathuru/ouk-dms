using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentManagement.Repository
{
    public  class RoleRepository : GenericRepository<Role, DocumentContext>,
          IRoleRepository
    {
        public RoleRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }

      
    }
}