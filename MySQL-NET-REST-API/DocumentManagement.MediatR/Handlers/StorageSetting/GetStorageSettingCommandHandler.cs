using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    // Renamed the class to match the Query naming convention
    public class GetStorageSettingQueryHandler : IRequestHandler<GetStorageSettingQuery, ServiceResponse<StorageSettingDto<object>>>
    {
        private readonly IStorageSettingRepository _storageSettingRepository;
        private readonly IMapper _mapper;

        public GetStorageSettingQueryHandler(
            IStorageSettingRepository storageSettingRepository,
            IMapper mapper)
        {
            _storageSettingRepository = storageSettingRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<StorageSettingDto<object>>> Handle(GetStorageSettingQuery request, CancellationToken cancellationToken)
        {
            var entity = await _storageSettingRepository.FindAsync(request.Id);

            if (entity == null)
            {
                return ServiceResponse<StorageSettingDto<object>>.Return409("Not found");
            }

            // Handle potential null or invalid JsonValue
            object jsonValue = null;
            if (!string.IsNullOrWhiteSpace(entity.JsonValue))
            {
                try
                {
                    jsonValue = JsonConvert.DeserializeObject<object>(entity.JsonValue);
                }
                catch (JsonException)
                {
                    return ServiceResponse<StorageSettingDto<object>>.Return409("Invalid JSON format in storage setting.");
                }
            }

            // Map entity to DTO
            var storageSetting = new StorageSettingDto<object>
            {
                Id = entity.Id,
                IsDefault = entity.IsDefault,
                EnableEncryption = entity.EnableEncryption,
                JsonValue = jsonValue,
                Name = entity.Name,
                StorageType = entity.StorageType
            };

            return ServiceResponse<StorageSettingDto<object>>.ReturnResultWith200(storageSetting);
        }
    }
}
