using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.UserNotification
{
    public class GetUserNotificationQueryHandler : IRequestHandler<GetUserNotificationQuery, List<UserNotificationDto>>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMapper _mapper;

        public GetUserNotificationQueryHandler(
           IUserNotificationRepository userNotificationRepository,
           UserInfoToken userInfoToken,
            IMapper mapper)
        {
            _userNotificationRepository = userNotificationRepository;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
        }

        public async Task<List<UserNotificationDto>> Handle(GetUserNotificationQuery request, CancellationToken cancellationToken)
        {
            var userId = _userInfoToken.Id;
            var today = DateTime.UtcNow;
            var fromDate = today.AddDays(-1).AddSeconds(1);
            var toDate = today.AddDays(1).AddSeconds(-1);
            var entities = await _userNotificationRepository
                .All.Include(c => c.Document)
                .Include(c=>c.WorkflowInstance)
                    .ThenInclude(c => c.Workflow)
                .Where(c => c.UserId == userId && !c.IsRead
                && (!c.DocumentId.HasValue || !c.Document.IsDeleted)
                && c.CreatedDate > fromDate && c.CreatedDate < toDate)
                .OrderByDescending(c => c.CreatedDate)
                .Take(10)
                 .Select(c => new UserNotificationDto
                 {
                     Id = c.Id,
                     CreatedDate = c.CreatedDate,
                     DocumentId = c.DocumentId,
                     DocumentName = c.Document.Name,
                     IsRead = c.IsRead,
                     Message = c.Message,
                     UserId = c.UserId,
                     Url = c.Document.Url,
                     NotificationsType = c.NotificationsType,
                     WorkflowName = c.WorkflowInstance.Workflow.Name,
                     WorkflowInstanceId = c.WorkflowInstanceId,
                     DocumentNumber = c.Document.DocumentNumber
                 })
                .ToListAsync();
            return _mapper.Map<List<UserNotificationDto>>(entities);
        }
    }
}
