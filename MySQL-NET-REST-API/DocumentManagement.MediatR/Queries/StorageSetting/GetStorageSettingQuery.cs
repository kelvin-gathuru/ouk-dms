using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetStorageSettingQuery : IRequest<ServiceResponse<StorageSettingDto<object>>>
    {
        public Guid Id { get; set; }
    }
}
