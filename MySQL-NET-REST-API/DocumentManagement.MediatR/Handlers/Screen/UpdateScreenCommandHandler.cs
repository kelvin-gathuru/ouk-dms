using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateScreenCommandHandler : IRequestHandler<UpdateScreenCommand, ScreenDto>
    {
        private readonly IScreenRepository _screenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public UpdateScreenCommandHandler(
           IScreenRepository screenRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _screenRepository = screenRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<ScreenDto> Handle(UpdateScreenCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _screenRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new ScreenDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Screen Name Already Exist." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<Screen>(request);
            entityExist = await _screenRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entity.CreatedBy = entityExist.CreatedBy;
            entity.CreatedDate = entityExist.CreatedDate;
            _screenRepository.Update(entity);
            if (await _uow.SaveAsync() <= -1)
            {
                var errorDto = new ScreenDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var entityDto = _mapper.Map<ScreenDto>(entity);
            return entityDto;
        }
    }
}
