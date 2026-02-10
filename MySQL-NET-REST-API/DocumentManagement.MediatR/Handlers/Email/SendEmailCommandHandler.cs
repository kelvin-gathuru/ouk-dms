using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly EmailHelper _emailHelper;
        private readonly ILogger<SendEmailCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public SendEmailCommandHandler(
           ISendEmailRepository sendEmailRepository,
           UserInfoToken userInfoToken,
           IMapper mapper,
           IUnitOfWork<DocumentContext> uow,
           EmailHelper emailHelper,
           ILogger<SendEmailCommandHandler> logger,
           IMediator mediator,
           IConfiguration configuration
            )
        {
            _sendEmailRepository = sendEmailRepository;
            _userInfoToken = userInfoToken;
            _mapper = mapper;
            _uow = uow;
            _emailHelper = emailHelper;
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }
        public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var sendEmail = _mapper.Map<SendEmail>(request);
            sendEmail.FromEmail = _userInfoToken.Email;
            sendEmail.IsSend = false;
            _sendEmailRepository.Add(sendEmail);
            if (await _uow.SaveAsync() <= -1)
            {
                return false;
            }

            // Try to send email immediately using appsettings.json credentials
            try
            {
                var smtpHost = _configuration["SmtpSettings:Host"];
                var smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");
                var smtpUserName = _configuration["SmtpSettings:UserName"];
                var smtpPassword = _configuration["SmtpSettings:Password"];
                var smtpEncryptionType = _configuration["SmtpSettings:EncryptionType"];
                var smtpFromEmail = _configuration["SmtpSettings:FromEmail"];
                var smtpFromName = _configuration["SmtpSettings:FromName"];

                if (!string.IsNullOrEmpty(smtpHost) && !string.IsNullOrEmpty(sendEmail.Email))
                {
                    var email = new SendEmailSpecification
                    {
                        UserName = smtpUserName,
                        Password = smtpPassword,
                        FromAddress = smtpFromEmail,
                        ToAddress = sendEmail.Email,
                        Body = sendEmail.Message,
                        Host = smtpHost,
                        Port = smtpPort,
                        Subject = sendEmail.Subject,
                        CCAddress = "",
                        EncryptionType = smtpEncryptionType,
                        FromName = smtpFromName
                    };

                    // Attach document if specified
                    if (sendEmail.DocumentId != null)
                    {
                        var downloadDocumentCommand = new DownloadDocumentCommand
                        {
                            Id = sendEmail.DocumentId.Value,
                            IsVersion = false
                        };
                        var response = await _mediator.Send(downloadDocumentCommand);
                        if (response.Success)
                        {
                            var documentDownload = response.Data;
                            var fileInfo = new Helper.FileInfo()
                            {
                                Src = documentDownload.Data,
                                FileType = documentDownload.ContentType,
                                Extension = Path.GetExtension(documentDownload.FileName),
                                Name = documentDownload.FileName
                            };
                            email.Attechments.Add(fileInfo);
                        }
                    }

                    // Send the email
                    await _emailHelper.SendEmail(email);
                    
                    // Mark as sent
                    sendEmail.IsSend = true;
                    _sendEmailRepository.Update(sendEmail);
                    await _uow.SaveAsync();
                    
                    _logger.LogInformation($"Email sent successfully to {sendEmail.Email}");
                }
                else
                {
                    _logger.LogWarning("Email queued but not sent immediately - SMTP settings not configured in appsettings.json");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email immediately. Email will be processed by background scheduler.");
                // Don't fail the request - email is saved and will be picked up by scheduler
            }

            return true;
        }
    }
}
