using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteScreenCommandHandler : IRequestHandler<DeleteScreenCommand, ScreenDto>
    {
        private readonly IScreenRepository _screenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        public DeleteScreenCommandHandler(
           IScreenRepository screenRepository,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _screenRepository = screenRepository;
            _uow = uow;
        }

        public async Task<ScreenDto> Handle(DeleteScreenCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _screenRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                var errorDto = new ScreenDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
                return errorDto;
            }
            _screenRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= -1)
            {
                var errorDto = new ScreenDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new ScreenDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Screen deleted successfully." }
            };
        }
    }
}
