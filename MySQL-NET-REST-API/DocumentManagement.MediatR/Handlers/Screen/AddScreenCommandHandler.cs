using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddScreenCommandHandler : IRequestHandler<AddScreenCommand, ScreenDto>
    {
        private readonly IScreenRepository _screenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddScreenCommandHandler> _logger;
        public AddScreenCommandHandler(
           IScreenRepository screenRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            ILogger<AddScreenCommandHandler> logger
            )
        {
            _screenRepository = screenRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ScreenDto> Handle(AddScreenCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _screenRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                var errorDto = new ScreenDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Screen Already Exist." }
                };
                _logger.LogWarning($"Screen Already Exist::{errorDto.Name}", errorDto);
                return errorDto;
            }
            var entity = _mapper.Map<Screen>(request);
            entity.Id = Guid.NewGuid();
            _screenRepository.Add(entity);
            if (await _uow.SaveAsync() <= -1)
            {
                var errorDto = new ScreenDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                _logger.LogError("Save Screen have Error", errorDto);
                return errorDto;
            }
            var entityDto = _mapper.Map<ScreenDto>(entity);
            return entityDto;
        }
    }
}
