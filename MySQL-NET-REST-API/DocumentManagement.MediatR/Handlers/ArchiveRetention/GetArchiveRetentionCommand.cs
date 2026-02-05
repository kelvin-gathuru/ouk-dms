using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class GetArchiveRetentionCommand : IRequest<ArchiveRetentionDto>
{
}
