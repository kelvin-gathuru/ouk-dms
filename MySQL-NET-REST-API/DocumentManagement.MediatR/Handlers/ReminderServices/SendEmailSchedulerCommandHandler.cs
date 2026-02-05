using Azure;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class SendEmailSchedulerCommandHandler : IRequestHandler<SendEmailSchedulerCommand, bool>
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly ILogger<SendEmailSchedulerCommandHandler> _logger;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly EmailHelper _emailHelper;
        private readonly IMediator _mediator;

        public SendEmailSchedulerCommandHandler(
            ISendEmailRepository sendEmailRepository,
            ILogger<SendEmailSchedulerCommandHandler> logger,
            IUnitOfWork<DocumentContext> uow,
            IEmailSMTPSettingRepository emailSMTPSettingRepository,
            EmailHelper emailHelper,
            IMediator mediator
            )
        {
            _sendEmailRepository = sendEmailRepository;
            _uow = uow;
            _logger = logger;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _emailHelper = emailHelper;
            _mediator = mediator;
        }

        public async Task<bool> Handle(SendEmailSchedulerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sendEmails = await _sendEmailRepository.All
                    .OrderByDescending(c => c.CreatedDate)
                    .Where(c => !c.IsSend)
                    .Take(10)
                    .ToListAsync();

                if (sendEmails.Count > 0)
                {
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    if (defaultSmtp == null)
                    {
                        return true;
                    }

                    foreach (var sendEmail in sendEmails)
                    {
                        if (!string.IsNullOrEmpty(sendEmail.Email))
                        {
                            try
                            {
                                var email = new SendEmailSpecification
                                {
                                    UserName = defaultSmtp.UserName,
                                    Password = defaultSmtp.Password,
                                    FromAddress = defaultSmtp.FromEmail,
                                    ToAddress = sendEmail.Email,
                                    Body = sendEmail.Message,
                                    Host = defaultSmtp.Host,
                                    Port = defaultSmtp.Port,
                                    Subject = sendEmail.Subject,
                                    CCAddress = "",
                                    EncryptionType = defaultSmtp.EncryptionType,
                                    FromName = defaultSmtp.FromName
                                };

                                if (sendEmail.DocumentId != null)
                                {
                                    var downloadDocumentCommand =new DownloadDocumentCommand
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
                                await _emailHelper.SendEmail(email);

                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message, ex);
                            }
                        }
                        sendEmail.FromEmail = defaultSmtp.UserName;
                        sendEmail.IsSend = true;
                    }
                    _sendEmailRepository.UpdateRange(sendEmails);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return true;
            }
        }
    }
}
