using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetUserNotificationCountQueryHandler : IRequestHandler<GetUserNotificationCountQuery, int>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly UserInfoToken _userInfoToken;

        public GetUserNotificationCountQueryHandler(
            IReminderSchedulerRepository reminderSchedulerRepository,
            UserInfoToken userInfoToken)
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _userInfoToken = userInfoToken;
        }
        public async Task<int> Handle(GetUserNotificationCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _reminderSchedulerRepository.All.CountAsync(c => c.UserId == _userInfoToken.Id && !c.IsRead);
            return count;
        }
    }
}
