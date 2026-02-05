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
    public class DeleteAllowFileExtensionCommandHandler(IAllowFileExtensionRepository _allowFileExtensionRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<DeleteAllowFileExtensionCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteAllowFileExtensionCommand request, CancellationToken cancellationToken)
        {
            var extension = await _allowFileExtensionRepository.FindAsync(request.Id);
            if (extension == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _allowFileExtensionRepository.Remove(extension);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}

