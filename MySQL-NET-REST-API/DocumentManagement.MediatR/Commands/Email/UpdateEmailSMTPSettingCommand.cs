using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateEmailSMTPSettingCommand : IRequest<ServiceResponse<EmailSMTPSettingDto>>
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool IsDefault { get; set; }
        public string EncryptionType { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
