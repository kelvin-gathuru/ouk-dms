using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllStorageSettingCommandHandler : IRequestHandler<GetAllStorageSettingQuery, List<StorageSettingDto<string>>>
    {
        private readonly IStorageSettingRepository _storageSettingRepository;
        private readonly IMapper _mapper;

        public GetAllStorageSettingCommandHandler(
           IStorageSettingRepository storageSettingRepository,
            IMapper mapper)
        {
            _storageSettingRepository = storageSettingRepository;
            _mapper = mapper;

        }

        public async Task<List<StorageSettingDto<string>>> Handle(GetAllStorageSettingQuery request, CancellationToken cancellationToken)
        {
            var entities = await _storageSettingRepository.All.Where(setting => setting.IsDeleted == false).ToListAsync(cancellationToken);
            return _mapper.Map<List<StorageSettingDto<string>>>(entities);
        }
    }
}
