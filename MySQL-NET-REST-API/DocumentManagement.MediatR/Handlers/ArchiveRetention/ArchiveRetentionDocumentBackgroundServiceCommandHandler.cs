using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
internal class ArchiveRetentionDocumentBackgroundServiceCommandHandler(
    IDocumentRepository documentRepository,
    IArchiveRetentionRepository archiveRetentionRepository,
    ICategoryRepository categoryRepository,
    IMediator mediator,
    IUserRepository userRepository,
    UserInfoToken userInfoToken) : IRequestHandler<ArchiveRetentionDocumentBackgroundServiceCommand, bool>
{
    public async Task<bool> Handle(ArchiveRetentionDocumentBackgroundServiceCommand request, CancellationToken cancellationToken)
    {
        var archiveRetention = await archiveRetentionRepository.All.FirstOrDefaultAsync();

        if (archiveRetention != null && archiveRetention.IsEnabled && archiveRetention.RetentionPeriodInDays > 0)
        {
            var retentionDate = DateTime.UtcNow.AddDays(-archiveRetention.RetentionPeriodInDays.Value);
            var documentsToArchive = await documentRepository.All
                .Where(d => d.ModifiedDate < retentionDate && d.IsArchive && !d.IsDeleted)
                .Select(d => new
                {
                    d.Id
                })
                .ToListAsync(cancellationToken);

            var categories = await categoryRepository.All.Where(c => c.IsArchive && c.ArchiveParentId == null && c.ModifiedDate < retentionDate)
               .Select(c => new
               {
                   c.Id
               })
               .ToListAsync();

            if (!documentsToArchive.Any() || categories.Any())
            {
                var systemUser = userRepository
           .FindBy(u => u.IsSystemUser && !u.IsDeleted)
           .FirstOrDefault();
                if (systemUser == null)
                {
                    return true; // No system user found, nothing to do
                }
                userInfoToken.Id = systemUser.Id;
                userInfoToken.Email = systemUser.Email;
                userInfoToken.IsSuperAdmin = systemUser.IsSuperAdmin;
            }

            if (documentsToArchive.Any())
            {
                foreach (var document in documentsToArchive)
                {
                    var deleteDocumentCommand = new DeleteDocumentCommand
                    {
                        Id = document.Id,
                        IsRetention = true // Assuming this is needed for retention logic
                    };
                    var flag = await mediator.Send(deleteDocumentCommand, cancellationToken);
                }
            }

            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    var deleteCategoryFromBackgroundServiceCommand = new DeleteCategoryFromBackgroundServiceCommand
                    {
                        CategoryId = category.Id,

                    };
                    var flag = await mediator.Send(deleteCategoryFromBackgroundServiceCommand, cancellationToken);
                }
            }
        }

        return true;
    }
}
