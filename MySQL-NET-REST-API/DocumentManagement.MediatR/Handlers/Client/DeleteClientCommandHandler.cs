using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteClientCommandHandler(IClientRepository _clientRepository,IDocumentRepository _documentRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<DeleteClientCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var isExistingDoc = await _documentRepository.All.AnyAsync(c => c.ClientId == request.Id);
            if (isExistingDoc)
            {
                return ServiceResponse<bool>.Return404("Client can not be deleted. Document is assign to this client.");
            }
            var extension = await _clientRepository.FindAsync(request.Id);
            if (extension == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _clientRepository.Delete(extension);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
