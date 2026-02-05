using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class AddUpdateArchiveRetentionCommand : IRequest<bool>
{
    public int? RetentionPeriodInDays { get; set; }
    public bool IsEnabled { get; set; }
}
