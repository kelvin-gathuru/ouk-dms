using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeletePageHelperCommandHandler : IRequestHandler<DeletePageHelperCommand, ServiceResponse<bool>>
    {
        private readonly IPageHelperRepository _pageHelperRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public DeletePageHelperCommandHandler(
           IPageHelperRepository pageHelperRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _pageHelperRepository = pageHelperRepository;
            _uow = uow;
        }
        public async Task<ServiceResponse<bool>> Handle(DeletePageHelperCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _pageHelperRepository.FindAsync(request.Id);

            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            }

            _pageHelperRepository.Delete(request.Id);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
