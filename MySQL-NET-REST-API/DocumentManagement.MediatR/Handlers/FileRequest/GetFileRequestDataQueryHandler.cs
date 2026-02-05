using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetFileRequestDataQueryHandler(IFileRequestsRepository _fileRequestsRepository) : IRequestHandler<GetFileRequestDataQuery, ServiceResponse<FileRequestDto>>
    {
        public async Task<ServiceResponse<FileRequestDto>> Handle(GetFileRequestDataQuery request, CancellationToken cancellationToken)

        {
            var entity = await _fileRequestsRepository.All
                .Include(c => c.CreatedBy)
                .Include(c => c.FileRequestDocuments)
               .FirstOrDefaultAsync(w => w.Id == request.Id);
            if (entity == null)
            {
                return ServiceResponse<FileRequestDto>.Return409("Not found");
            }
            bool isMaxDocumentReached = entity.FileRequestDocuments.Count >= entity.MaxDocument;
            bool isLinkExpired = isMaxDocumentReached || DateTime.UtcNow > entity.LinkExpiryTime;
            if(isLinkExpired)
            {
                return ServiceResponse<FileRequestDto>.Return409("Link is expried.");
            }

            var fileRequestDocuments = new List<FileRequestDocumentDto>();
            foreach (var item in entity.FileRequestDocuments)
            {
                var fileRequestDocument = new FileRequestDocumentDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Url = item.Url,
                    CreatedDate = item.CreatedDate,
                    FileRequestId = item.FileRequestId,
                    FileRequestDocumentStatus = item.FileRequestDocumentStatus,
                };
                fileRequestDocuments.Add(fileRequestDocument);
            }
            var entityDto = new FileRequestDto
            {
                Id = entity.Id,
                Subject = entity.Subject,
                Email = entity.Email,
                SizeInMb = entity.SizeInMb,
                MaxDocument = entity.MaxDocument,
                FileRequestStatus = entity.FileRequestStatus,
                CreatedById = entity.CreatedBy != null ? $"{entity.CreatedBy.FirstName} {entity.CreatedBy.LastName}" : null,
                AllowExtension = entity.AllowExtension,
                CreatedDate = entity.CreatedDate,
                IsLinkExpired = DateTime.UtcNow > entity.LinkExpiryTime,
                LinkExpiryTime = entity.LinkExpiryTime,
                HasPassword = !string.IsNullOrWhiteSpace(entity.Password) ? true : false,
                FileRequestDocuments = fileRequestDocuments
            };
            return ServiceResponse<FileRequestDto>.ReturnResultWith200(entityDto);
        }
    }
}
