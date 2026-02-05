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
    public class DeleteStorageSettingCommandHandler : IRequestHandler<DeleteStorageSettingCommand, ServiceResponse<bool>>
    {
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IStorageSettingRepository _storageSettingRepository;

        public DeleteStorageSettingCommandHandler(IUnitOfWork<DocumentContext> uow, IStorageSettingRepository storageSettingRepository)
        {
            _uow = uow;
            _storageSettingRepository = storageSettingRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteStorageSettingCommand request, CancellationToken cancellationToken)
        {
            var storageSetting = await _storageSettingRepository.FindAsync(request.Id);
            if (storageSetting == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            storageSetting.IsDeleted = true;
            _storageSettingRepository.Delete(storageSetting);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
