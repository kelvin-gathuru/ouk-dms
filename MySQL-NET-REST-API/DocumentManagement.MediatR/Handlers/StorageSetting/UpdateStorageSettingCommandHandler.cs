using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateStorageSettingCommandHandler : IRequestHandler<UpdateStorageSettingCommand, ServiceResponse<StorageSettingDto<string>>>
    {
        private readonly IStorageSettingRepository _storageSettingRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public UpdateStorageSettingCommandHandler(
            IStorageSettingRepository storageSettingRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow)
        {
            _storageSettingRepository = storageSettingRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResponse<StorageSettingDto<string>>> Handle(UpdateStorageSettingCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await _storageSettingRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id).FirstOrDefaultAsync();
            if (nameExists != null)
            {
                return ServiceResponse<StorageSettingDto<string>>.Return409("Storage Setting Name is already exists.");
            }
            var existingEntity = await _storageSettingRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                return ServiceResponse<StorageSettingDto<string>>.Return404("No record found.");
            }

            if (request.IsDefault)
            {
                var isDefaultEntity = _storageSettingRepository.All.Where(c => c.IsDefault == true && c.Id != request.Id).FirstOrDefault();
                if (isDefaultEntity != null)
                {
                    isDefaultEntity.IsDefault = false;
                    _storageSettingRepository.Update(isDefaultEntity);
                }
            }
            else
            {
                if (existingEntity.IsDefault)
                {
                    var localEntity = _storageSettingRepository.All.Where(c => c.StorageType == Data.StorageType.LOCAL_STORAGE).FirstOrDefault();
                    if (localEntity != null && localEntity.Id == existingEntity.Id)
                    {
                        request.IsDefault = true;
                    }
                    else
                    {
                        localEntity.IsDefault = true;
                        _storageSettingRepository.Update(localEntity);
                    }
                }
            }

            // If no issues, proceed with further logic...
            var storageSetting = _mapper.Map(request, existingEntity);
            _storageSettingRepository.Update(storageSetting);
            if (await _uow.SaveAsync() <= -1)
            {
                return ServiceResponse<StorageSettingDto<string>>.Return500();
            }
            return ServiceResponse<StorageSettingDto<string>>.ReturnResultWith201(_mapper.Map<StorageSettingDto<string>>(existingEntity));
        }
    }
}
