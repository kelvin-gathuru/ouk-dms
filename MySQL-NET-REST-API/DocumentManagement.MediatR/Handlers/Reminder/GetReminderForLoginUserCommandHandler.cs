using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetReminderForLoginUserCommandHandler : IRequestHandler<GetReminderForLoginUserCommand, ReminderList>
    {
        private readonly IReminderRepository _reminderRepository;
        public GetReminderForLoginUserCommandHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        public async Task<ReminderList> Handle(GetReminderForLoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _reminderRepository.GetRemindersForLoginUser(request.ReminderResource);
        }
    }
}
