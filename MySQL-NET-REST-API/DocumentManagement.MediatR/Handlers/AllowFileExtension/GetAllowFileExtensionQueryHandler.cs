using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetAllowFileExtensionQueryHandler(IAllowFileExtensionRepository _allowFileExtensionRepository) : IRequestHandler<GetAllowFileExtensionQuery, ServiceResponse<AllowFileExtensionDto>>
{
    public async Task<ServiceResponse<AllowFileExtensionDto>> Handle(GetAllowFileExtensionQuery request, CancellationToken cancellationToken)
    {
        var entity = await _allowFileExtensionRepository.All
           .FirstOrDefaultAsync(w => w.Id == request.Id);
        if (entity == null)
        {
            return ServiceResponse<AllowFileExtensionDto>.Return409("Not found");
        }

        var entityDto = new AllowFileExtensionDto
        {
            Extension = entity.Extension,
            Extensions = string.IsNullOrWhiteSpace(entity.Extension) ? new List<string>() : entity.Extension.Split(",").ToList(),
            FileType = entity.FileType,
            Id = entity.Id
        };

        return ServiceResponse<AllowFileExtensionDto>.ReturnResultWith200(entityDto);
    }
}
