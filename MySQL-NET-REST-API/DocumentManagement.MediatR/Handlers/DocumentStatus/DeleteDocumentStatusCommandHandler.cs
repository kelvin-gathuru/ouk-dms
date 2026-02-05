using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteDocumentStatusCommandHandler(IDocumentStatusRepository _documentStatusRepository,IDocumentRepository _documentRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<DeleteDocumentStatusCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteDocumentStatusCommand request, CancellationToken cancellationToken)
        {
            var isExistingDoc = await _documentRepository.All.AnyAsync(c => c.DocumentStatusId == request.Id);
            if (isExistingDoc)
            {
                return ServiceResponse<bool>.Return404("Document Status can not be deleted. Document is assign to this status.");
            }

            var storageSetting = await _documentStatusRepository.FindAsync(request.Id);
            if (storageSetting == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            
            _documentStatusRepository.Remove(storageSetting);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
