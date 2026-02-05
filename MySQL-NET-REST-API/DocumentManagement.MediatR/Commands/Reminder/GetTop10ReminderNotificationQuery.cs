using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.CommandAndQuery
{
   public class GetTop10ReminderNotificationQuery: IRequest<List<ReminderSchedulerDto>>
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
