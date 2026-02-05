using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteFileRequestCommandHandler(IFileRequestsRepository _fileRequestsRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<DeleteFileRequestCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteFileRequestCommand request, CancellationToken cancellationToken)
        {
            var fileRequest = await _fileRequestsRepository.FindAsync(request.Id);
            if (fileRequest == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            fileRequest.IsDeleted = true;
            _fileRequestsRepository.Update(fileRequest);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
