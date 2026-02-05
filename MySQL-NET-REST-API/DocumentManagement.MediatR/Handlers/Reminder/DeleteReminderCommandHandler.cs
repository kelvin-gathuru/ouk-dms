using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand, ServiceResponse<bool>>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public DeleteReminderCommandHandler(IReminderRepository reminderRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _reminderRepository = reminderRepository;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await _reminderRepository.FindAsync(request.Id);
            if (reminder == null)
            {
                return ServiceResponse<bool>.Return404();
            }

            reminder.IsDeleted = true;
            _reminderRepository.Update(reminder);
            if (await _uow.SaveAsync() <= -1)
            {
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
