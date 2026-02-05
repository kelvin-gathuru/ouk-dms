using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
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
    public class UpdateEmailSMTPSettingCommandHandler : IRequestHandler<UpdateEmailSMTPSettingCommand, ServiceResponse<EmailSMTPSettingDto>>
    {
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddEmailSMTPSettingCommand> _logger;
        public UpdateEmailSMTPSettingCommandHandler(
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
        public async Task<ServiceResponse<EmailSMTPSettingDto>> Handle(UpdateEmailSMTPSettingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _emailSMTPSettingRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Email SMTP setting does not exist.");
                return ServiceResponse<EmailSMTPSettingDto>.Return409("Email SMTP setting does not exist.");
            }

            if (request.Password == "*****")
            {
                request.Password = entityExist.Password;
            }

            entityExist = _mapper.Map(request, entityExist);

            // remove other as default
            if (entityExist.IsDefault)
            {
                var defaultEmailSMTPSettings = await _emailSMTPSettingRepository.All.Where(c => c.Id != request.Id && c.IsDefault).ToListAsync();
                defaultEmailSMTPSettings.ForEach(c => c.IsDefault = false);
                _emailSMTPSettingRepository.UpdateRange(defaultEmailSMTPSettings);
            }
            else
            {
                var count = await _emailSMTPSettingRepository.All.CountAsync();
                if (count == 1)
                {
                    entityExist.IsDefault = true;
                }
            }
            _emailSMTPSettingRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= -1)
            {
                return ServiceResponse<EmailSMTPSettingDto>.Return500();
            }
            var entityDto = _mapper.Map<EmailSMTPSettingDto>(entityExist);
            return ServiceResponse<EmailSMTPSettingDto>.ReturnResultWith200(entityDto);
        }
    }
}
