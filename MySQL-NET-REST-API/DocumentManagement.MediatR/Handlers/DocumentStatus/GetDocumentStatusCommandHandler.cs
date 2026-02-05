using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentStatusCommandHandler(IDocumentStatusRepository _documentStatusRepository) : IRequestHandler<GetDocumentStatusQuery, ServiceResponse<DocumentStatusDto>>
    {
        public async Task<ServiceResponse<DocumentStatusDto>> Handle(GetDocumentStatusQuery request, CancellationToken cancellationToken)
        {
            var entity = await _documentStatusRepository.FindAsync(request.Id);

            if (entity == null)
            {
                return ServiceResponse<DocumentStatusDto>.Return409("Not found");
            }

            // Map entity to DTO
            var documentStatus = new DocumentStatusDto
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name
            };

            return ServiceResponse<DocumentStatusDto>.ReturnResultWith200(documentStatus);
        }
    }
}
