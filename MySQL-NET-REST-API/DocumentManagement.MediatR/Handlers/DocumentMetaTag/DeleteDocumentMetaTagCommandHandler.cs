using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class DeleteDocumentMetaTagCommandHandler(IDocumentMetaTagRepository _documentMetaTagRepository, IUnitOfWork<DocumentContext> _uow) : IRequestHandler<DeleteDocumentMetaTagCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(DeleteDocumentMetaTagCommand request, CancellationToken cancellationToken)
    {
        var extension = await _documentMetaTagRepository.FindAsync(request.Id);
        if (extension == null)
        {
            return ServiceResponse<bool>.Return404();
        }
        _documentMetaTagRepository.Delete(extension);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.Return500();
        }
        return ServiceResponse<bool>.ReturnResultWith200(true);
    }
}
