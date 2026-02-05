using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetEmailSMTPSettingQueryHandler : IRequestHandler<GetEmailSMTPSettingQuery, ServiceResponse<EmailSMTPSettingDto>>
    {
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoleQueryHandler> _logger;

        public GetEmailSMTPSettingQueryHandler(
           IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IMapper mapper,
            ILogger<GetRoleQueryHandler> logger)
        {
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<EmailSMTPSettingDto>> Handle(GetEmailSMTPSettingQuery request, CancellationToken cancellationToken)
        {
            var entity = await _emailSMTPSettingRepository.All.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Password = "*****";
                return ServiceResponse<EmailSMTPSettingDto>.ReturnResultWith200(_mapper.Map<EmailSMTPSettingDto>(entity));
            }

            _logger.LogError("Not found");
            return ServiceResponse<EmailSMTPSettingDto>.Return404();
        }
    }
}
