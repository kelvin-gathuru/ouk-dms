using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteReminderCurrentUserCommandHandler : IRequestHandler<DeleteReminderCurrentUserCommand, ServiceResponse<bool>>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IReminderUserRepository _reminderUserRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly UserInfoToken _userInfoToken;

        public DeleteReminderCurrentUserCommandHandler(
            IReminderRepository reminderRepository,
            IUnitOfWork<DocumentContext> uow,
             IReminderUserRepository reminderUserRepository,
             UserInfoToken userInfoToken)
        {
            _reminderRepository = reminderRepository;
            _uow = uow;
            _reminderUserRepository = reminderUserRepository;
            _userInfoToken = userInfoToken;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteReminderCurrentUserCommand request, CancellationToken cancellationToken)
        {
            var reminder = await _reminderRepository.FindAsync(request.Id);
            if (reminder == null)
            {
                return ServiceResponse<bool>.Return404();
            }

            var reminserUser = await _reminderUserRepository.FindBy(c => c.ReminderId == request.Id && c.UserId == _userInfoToken.Id)
                                .FirstOrDefaultAsync();

            if (reminserUser != null)
            {
                _reminderUserRepository.Remove(reminserUser);
            }
            else
            {
                return ServiceResponse<bool>.Return404();
            }
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
