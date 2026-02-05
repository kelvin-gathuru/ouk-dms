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
public class RunArchiveRetensionOnDocumentActionConfigurationCommandHandler(
    IDocumentRepository documentRepository,
    IUserRepository userRepository,
    IMediator mediator,
    UserInfoToken userInfoToken) : IRequestHandler<RunArchiveRetensionOnDocumentActionConfigurationCommand, bool>
{
    public async Task<bool> Handle(RunArchiveRetensionOnDocumentActionConfigurationCommand request, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.UtcNow.Date;
        DateOnly targetDate = new DateOnly(currentDate.Year, currentDate.Month, currentDate.Day);
        var documents = await documentRepository
            .FindBy(d => d.RetentionDate != null && d.RetentionPeriodInDays > 0 && d.RetentionDate <= targetDate && !d.IsDeleted)
            .Select(d => new
            {
                d.Id,
                d.RetentionPeriodInDays,
                d.RetentionDate,
                d.OnExpiryAction
            })
            .ToListAsync(cancellationToken);

        if (documents.Any())
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

            foreach (var document in documents)
            {
                if (document.OnExpiryAction == Data.Entities.RETENTION_ACTION_ENUM.ARCHIVE)
                {
                    var archiveCommand = new ArchiveDocumentCommand
                    {
                        DocumentId = document.Id,
                        IsRetention = true
                    };
                    await mediator.Send(archiveCommand, cancellationToken);
                }
                else if (document.OnExpiryAction == Data.Entities.RETENTION_ACTION_ENUM.DELETE)
                {
                    var deleteCommand = new DeleteDocumentCommand
                    {
                        Id = document.Id,
                        IsRetention = true
                    };
                    await mediator.Send(deleteCommand, cancellationToken);
                }
            }

        }

        return true;
    }
}
