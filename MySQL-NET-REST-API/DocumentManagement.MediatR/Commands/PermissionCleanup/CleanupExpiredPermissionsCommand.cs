using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class CleanupExpiredPermissionsCommand : IRequest<bool>
{
}
