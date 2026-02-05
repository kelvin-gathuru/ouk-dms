using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;
internal class AddUpdateArchiveRetentionCommandHandler(IUnitOfWork<DocumentContext> _uow, IArchiveRetentionRepository archiveRetentionRepository) : IRequestHandler<AddUpdateArchiveRetentionCommand, bool>
{
    public async Task<bool> Handle(AddUpdateArchiveRetentionCommand request, CancellationToken cancellationToken)
    {
        var archiveRetention = await archiveRetentionRepository.All.FirstOrDefaultAsync();
        if (archiveRetention != null)
        {
            archiveRetention.RetentionPeriodInDays = request.RetentionPeriodInDays;
            archiveRetention.IsEnabled = request.IsEnabled;
            archiveRetentionRepository.Update(archiveRetention);
        }
        else
        {
            archiveRetention = new Data.Entities.ArchiveRetention
            {
                Id = Guid.NewGuid(),
                RetentionPeriodInDays = request.RetentionPeriodInDays,
                IsEnabled = request.IsEnabled
            };
            archiveRetentionRepository.Add(archiveRetention);
        }
        if (await _uow.SaveAsync() <= 0)
        {
            return false;
        }
        return true;
    }
}
