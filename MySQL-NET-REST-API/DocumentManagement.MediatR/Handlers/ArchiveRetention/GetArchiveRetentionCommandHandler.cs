using DocumentManagement.Data.Dto;
using DocumentManagement.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
internal class GetArchiveRetentionCommandHandler(IArchiveRetentionRepository archiveRetentionRepository) : IRequestHandler<GetArchiveRetentionCommand, ArchiveRetentionDto>
{
    public async Task<ArchiveRetentionDto> Handle(GetArchiveRetentionCommand request, CancellationToken cancellationToken)
    {
        var archiveRetention = archiveRetentionRepository.All.FirstOrDefault();
        if (archiveRetention != null)
        {
            return new ArchiveRetentionDto
            {
                RetentionPeriodInDays = archiveRetention.RetentionPeriodInDays,
                Id = archiveRetention.Id,
                IsEnabled = archiveRetention.IsEnabled
            };
        }
        return null;
    }
}
