using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class CheckReminderByDocumentCommandHandler(IReminderRepository _reminderRepository, UserInfoToken _userInfo) : IRequestHandler<CheckReminderByDocumentCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(CheckReminderByDocumentCommand request, CancellationToken cancellationToken)
        {
            var flag = await _reminderRepository.All.AnyAsync(c => c.DocumentId == request.DocumentId && c.ReminderUsers.Any(ru => ru.UserId == _userInfo.Id));
            return ServiceResponse<bool>.ReturnResultWith200(flag);
        }
    }
}
