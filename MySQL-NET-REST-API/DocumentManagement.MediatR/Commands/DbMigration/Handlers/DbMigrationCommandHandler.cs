using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DbMigrationCommandHandler : IRequestHandler<DbMigrationCommand, bool>
    {

        private readonly IUnitOfWork<DocumentContext> _uow;
        public DbMigrationCommandHandler(
            IUnitOfWork<DocumentContext> uow
            )
        {
            _uow = uow;
        }
        public async Task<bool> Handle(DbMigrationCommand request, CancellationToken cancellationToken)
        {
            await _uow.Context.Database.MigrateAsync();
            return true;
        }
    }
}
