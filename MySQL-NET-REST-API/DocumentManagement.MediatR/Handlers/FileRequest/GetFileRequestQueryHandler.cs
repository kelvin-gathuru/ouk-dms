using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetFileRequestQueryHandler (IFileRequestsRepository _fileRequestsRepository): IRequestHandler<GetFileRequestQuery, ServiceResponse<FileRequestDto>>
    {
        public async Task<ServiceResponse<FileRequestDto>> Handle(GetFileRequestQuery request, CancellationToken cancellationToken)
        
        {
            var entity = await _fileRequestsRepository.All
                .Include(c => c.CreatedBy)
               .FirstOrDefaultAsync(w => w.Id == request.Id);
            if (entity == null)
            {
                return ServiceResponse<FileRequestDto>.Return409("Not found");
            }
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                var base64EncodedBytes = Convert.FromBase64String(entity.Password);
                entity.Password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            var entityDto = new FileRequestDto
            {
                Id = entity.Id,
                Subject = entity.Subject,
                Email = entity.Email,
                SizeInMb = entity.SizeInMb,
                MaxDocument = entity.MaxDocument,
                Password = entity.Password,
                FileRequestStatus = entity.FileRequestStatus,
                CreatedById = entity.CreatedBy != null ? $"{entity.CreatedBy.FirstName} {entity.CreatedBy.LastName}" : null,
                AllowExtension = entity.AllowExtension,
                CreatedDate = entity.CreatedDate,
                IsLinkExpired = DateTime.UtcNow > entity.LinkExpiryTime,
                LinkExpiryTime = entity.LinkExpiryTime,
                HasPassword = !string.IsNullOrWhiteSpace(entity.Password) ? true : false,
            };
            return ServiceResponse<FileRequestDto>.ReturnResultWith200(entityDto);
        }
    }
}
