using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetEmailSMTPSettingCommandHandler(IEmailSMTPSettingRepository _emailSMTPSettingRepository,ILogger<GetEmailSMTPSettingCommandHandler> _logger) : IRequestHandler<GetEmailSMTPSettingCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(GetEmailSMTPSettingCommand request, CancellationToken cancellationToken)
        {
            var recordExists = await _emailSMTPSettingRepository.All.AnyAsync(cancellationToken);

            if (recordExists)
            {
                return ServiceResponse<bool>.ReturnResultWith200(true); 
            }
            else
            {
                _logger.LogWarning("No Email SMTP Settings found.");
                return ServiceResponse<bool>.Return404();
            }
        }

    }
}
