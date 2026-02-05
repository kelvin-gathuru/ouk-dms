using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class NotificationList : List<UserNotificationDto>
    {
        public NotificationList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public NotificationList(List<UserNotificationDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<NotificationList> Create(IQueryable<UserNotification> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new NotificationList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<UserNotification> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<UserNotificationDto>> GetDtos(IQueryable<UserNotification> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new UserNotificationDto
                {
                    Id = c.Id,
                    CreatedDate = c.CreatedDate,
                    DocumentId = c.DocumentId,
                    DocumentName = c.Document.Name,
                    DocumentNumber = c.Document.DocumentNumber,
                    IsRead = c.IsRead,
                    Message = c.Message,
                    UserId = c.UserId,
                    Url = c.Document.Url,
                    NotificationsType = c.NotificationsType,
                    WorkflowName = c.WorkflowInstance.Workflow.Name,
                    WorkflowInstanceId= c.WorkflowInstanceId

                })
                .ToListAsync();
            return entities;
        }
    }
}
