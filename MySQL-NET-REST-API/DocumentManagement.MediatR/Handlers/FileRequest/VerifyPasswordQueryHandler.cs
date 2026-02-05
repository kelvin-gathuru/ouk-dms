using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class VerifyPasswordQueryHandler (IFileRequestsRepository _fileRequestsRepository) : IRequestHandler<VerifyPasswordQuery, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(VerifyPasswordQuery request, CancellationToken cancellationToken)
        {
            var entity = await _fileRequestsRepository.All
               .FirstOrDefaultAsync(w => w.Id == request.Id);
            if (entity == null)
            {
                return ServiceResponse<bool>.Return409("Not found");
            }
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                var base64EncodedBytes = Convert.FromBase64String(entity.Password);
                entity.Password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            if(entity.Password == request.Password)
            {
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            return ServiceResponse<bool>.ReturnResultWith200(false);
        }
    }
}
