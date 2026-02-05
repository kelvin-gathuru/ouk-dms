using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdatePageHelperCommandHandler : IRequestHandler<UpdatePageHelperCommand, ServiceResponse<PageHelperDto>>
    {
        private readonly IPageHelperRepository _pageHelperRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public UpdatePageHelperCommandHandler(IPageHelperRepository pageHelperRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow)
        {
            _pageHelperRepository = pageHelperRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResponse<PageHelperDto>> Handle(UpdatePageHelperCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _pageHelperRepository.All.FirstOrDefaultAsync(c => c.Name.ToUpper() == request.Name.ToUpper() && c.Id != request.Id);
            if (entityExist != null)
            {
                return ServiceResponse<PageHelperDto>.Return409("Page Helper with same name already exists.");
            }

            entityExist = await _pageHelperRepository.All.FirstOrDefaultAsync(c => c.Id == request.Id);

            if (entityExist == null)
            {
                return ServiceResponse<PageHelperDto>.Return409("Page Helper does not exists.");
            }

            entityExist.Name = request.Name;
            entityExist.Description = request.Description;
            _pageHelperRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PageHelperDto>.Return500();
            }

            var entityDto = _mapper.Map<PageHelperDto>(entityExist);
            return ServiceResponse<PageHelperDto>.ReturnResultWith200(entityDto);
        }
    }
}
