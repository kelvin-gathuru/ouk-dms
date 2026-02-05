using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands.Email;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers.Email
{
    public class SendTestEmailCommandHandler(
        ILogger<SendTestEmailCommandHandler> logger,
        EmailHelper emailHelper
        ) : IRequestHandler<SendTestEmailCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(SendTestEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var data = new SendEmailSpecification
                {
                    Body = "Dear User,\n\nThis is a test email to verify the SMTP configuration. If you're receiving this email, the SMTP settings are working correctly.\n\nBest regards",
                    FromAddress = request.FromEmail,
                    FromName = request.FromName,
                    Host = request.Host,
                    EncryptionType = request.EncryptionType,
                    Password = request.Password,
                    Port = request.Port,
                    Subject = "SMTP Configuration Test",
                    ToAddress = request.ToEmail,
                    CCAddress = "",
                    UserName = request.UserName
                };

                var emailTestResult = await emailHelper.SendEmail(data);
                if (!emailTestResult)
                {
                    return ServiceResponse<bool>.Return422("SMTP configuration is incorrect, please check the settings and try again.");
                }
                return ServiceResponse<bool>.ReturnSuccess();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while sending test email");
                return ServiceResponse<bool>.Return422("SMTP configuration is incorrect, please check the settings and try again.");
            }
        }
    }
}
