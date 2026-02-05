using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class GetEmailSMTPSettingQuery : IRequest<ServiceResponse<EmailSMTPSettingDto>>
    {
        public Guid Id { get; set; }
    }
}
