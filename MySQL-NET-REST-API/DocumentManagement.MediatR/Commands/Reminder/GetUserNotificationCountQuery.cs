using MediatR;

namespace DocumentManagement.MediatR.CommandAndQuery
{
    public class GetUserNotificationCountQuery: IRequest<int>
    {
    }
}
