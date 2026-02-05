using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddEmailSMTPSettingCommandHandler : IRequestHandler<AddEmailSMTPSettingCommand, ServiceResponse<EmailSMTPSettingDto>>
    {
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddEmailSMTPSettingCommand> _logger;

        public AddEmailSMTPSettingCommandHandler(
           IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            ILogger<AddEmailSMTPSettingCommand> logger
        )
        {
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<EmailSMTPSettingDto>> Handle(AddEmailSMTPSettingCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<EmailSMTPSetting>(request);
            _emailSMTPSettingRepository.Add(entity);

            if (entity.IsDefault)
            {
                var defaultEmailSMTPSettings = await _emailSMTPSettingRepository.All.Where(c => c.IsDefault).ToListAsync();
                defaultEmailSMTPSettings.ForEach(c => c.IsDefault = false);
                _emailSMTPSettingRepository.UpdateRange(defaultEmailSMTPSettings);
            }

            if (await _uow.SaveAsync() <= -1)
            {
                _logger.LogError("Failed to save email SMTP settings.");
                return ServiceResponse<EmailSMTPSettingDto>.Return409("Failed to save email SMTP settings.");
            }

            var entityDto = _mapper.Map<EmailSMTPSettingDto>(entity);
            return ServiceResponse<EmailSMTPSettingDto>.ReturnResultWith200(entityDto);
        }
    }
}
