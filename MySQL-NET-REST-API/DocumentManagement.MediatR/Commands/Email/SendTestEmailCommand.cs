using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands.Email
{
    public class SendTestEmailCommand : IRequest<ServiceResponse<bool>>
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string EncryptionType { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ToEmail { get; set; }
    }
}
