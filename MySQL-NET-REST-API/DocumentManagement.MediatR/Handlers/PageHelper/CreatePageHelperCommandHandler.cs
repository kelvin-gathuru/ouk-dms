using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class CreatePageHelperCommandHandler : IRequestHandler<CreatePageHelperCommand, ServiceResponse<PageHelperDto>>
    {
        private readonly IPageHelperRepository _pageHelperRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public CreatePageHelperCommandHandler(IPageHelperRepository pageHelperRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow)
        {
            _pageHelperRepository = pageHelperRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<ServiceResponse<PageHelperDto>> Handle(CreatePageHelperCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _pageHelperRepository.FindBy(c => c.Name.ToUpper() == request.Name.ToUpper()).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<PageHelperDto>.Return409("Page helper with same name already exists.");
            }

            var entity = _mapper.Map<PageHelper>(request);
            entity.Code = entity.Name.ToUpper().Replace(" ", "_");
            entity.Id = Guid.NewGuid();

            _pageHelperRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PageHelperDto>.Return500();
            }
            var entityDto = _mapper.Map<PageHelperDto>(entity);
            return ServiceResponse<PageHelperDto>.ReturnResultWith201(entityDto);
        }
    }
}
