using AutoMapper;
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

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllAllowFileExtensionQueryHandler(IAllowFileExtensionRepository _allowFileExtensionRepository, IMapper _mapper) : IRequestHandler<GetAllAllowFileExtensionQuery, ServiceResponse<List<AllowFileExtensionDto>>>
    {
        public async Task<ServiceResponse<List<AllowFileExtensionDto>>> Handle(GetAllAllowFileExtensionQuery request, CancellationToken cancellationToken)
        {
            var entities = await _allowFileExtensionRepository.All
                .ToListAsync(cancellationToken);

            if (entities.Count == 0)
            {
                return ServiceResponse<List<AllowFileExtensionDto>>.ReturnResultWith200([]);
            }
            var entityDtos = entities.Select(c => new AllowFileExtensionDto
            {
                Extension = c.Extension,
                Extensions = string.IsNullOrWhiteSpace(c.Extension) ? new List<string>() : c.Extension.Split(",").ToList(),
                FileType = c.FileType,
                Id= c.Id
            }).ToList();

            return ServiceResponse<List<AllowFileExtensionDto>>.ReturnResultWith200(entityDtos);
        }
    }
}
