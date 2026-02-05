using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ClientForgotPasswordCommandHandler : IRequestHandler<ClientForgotPasswordCommand, ServiceResponse<string>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly EmailHelper _emailHelper;
        private readonly IConfiguration _configuration;

        public ClientForgotPasswordCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork<DocumentContext> uow,
            EmailHelper emailHelper,
            IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _uow = uow;
            _emailHelper = emailHelper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Handle(ClientForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.FindBy(c => c.Email == request.Email).FirstOrDefaultAsync();
            if (client == null)
            {
                // Return success even if email not found to prevent enumeration, or return 404 if less strict
                // For this internal/specific app, 404 is fine or just a generic message.
                return ServiceResponse<string>.ReturnFailed(404, "Email not found.");
            }

            // Generate Token (Reuse ActivationCode)
            string token = new Random().Next(100000, 999999).ToString();
            client.ActivationCode = token;
            _clientRepository.Update(client);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<string>.Return500();
            }

            // Send Email
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            bool emailSent = false;
            try
            {
                var sendEmailSpecification = new SendEmailSpecification
                {
                    FromAddress = smtpSettings["FromEmail"],
                    FromName = smtpSettings["FromName"],
                    ToAddress = client.Email,
                    ToName = client.ContactPerson,
                    Subject = "Reset your Parliament System Password",
                    Body = $@"
                        <h3>Password Reset Request</h3>
                        <p>Dear {client.ContactPerson},</p>
                        <p>You requested a password reset. Please use the following code to reset your password:</p>
                        <h2>{token}</h2>
                        <p>If you did not request this, please ignore this email.</p>",
                    Host = smtpSettings["Host"],
                    Port = int.Parse(smtpSettings["Port"]),
                    UserName = smtpSettings["UserName"],
                    Password = smtpSettings["Password"],
                    EncryptionType = smtpSettings["EncryptionType"]
                };

                emailSent = await _emailHelper.SendEmail(sendEmailSpecification);
                if (!emailSent)
                {
                     return ServiceResponse<string>.ReturnFailed(500, "Failed to send email. Check server logs.");
                }
            }
            catch (Exception ex)
            {
                return ServiceResponse<string>.ReturnFailed(500, $"Error sending email: {ex.Message} {ex.InnerException?.Message}");
            }

            // Return the token for testing purposes since email might not work in dev env
            return ServiceResponse<string>.ReturnResultWith200(token, $"Code sent to {client.Email} via {smtpSettings["Host"]}:{smtpSettings["Port"]}. Result: {emailSent}");
        }
    }
}
