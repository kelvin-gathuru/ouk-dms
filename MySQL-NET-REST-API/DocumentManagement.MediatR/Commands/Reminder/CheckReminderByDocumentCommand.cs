using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.CommandAndQuery
{
    public class CheckReminderByDocumentCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
    }
}
