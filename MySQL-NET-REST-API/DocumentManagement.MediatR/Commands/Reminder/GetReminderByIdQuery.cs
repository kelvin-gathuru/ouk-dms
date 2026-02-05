using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.CommandAndQuery
{
    public class GetReminderByIdQuery : IRequest<ServiceResponse<ReminderDto>>
    {
        public Guid Id { get; set; }
    }
}
