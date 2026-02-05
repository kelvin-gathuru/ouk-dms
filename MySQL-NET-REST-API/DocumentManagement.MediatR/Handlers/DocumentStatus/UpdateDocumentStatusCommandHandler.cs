using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
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
    public class UpdateDocumentStatusCommandHandler(IDocumentStatusRepository _documentStatusRepository, IMapper _mapper,IUnitOfWork<DocumentContext> _uow) : IRequestHandler<UpdateDocumentStatusCommand, ServiceResponse<DocumentStatusDto>>
    {
        public async Task<ServiceResponse<DocumentStatusDto>> Handle(UpdateDocumentStatusCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await _documentStatusRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id   ).FirstOrDefaultAsync();
            var entity = await _documentStatusRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();

            if (nameExists != null && entity == null)
            {
                return ServiceResponse<DocumentStatusDto>.Return409("Invalid Id. The Name exists, but no matching Id found.");
            }
            else if (nameExists != null)
            {
                return ServiceResponse<DocumentStatusDto>.Return409("DocumentStatus Name already exists.");
            }
            else if (entity == null)
            {
                return ServiceResponse<DocumentStatusDto>.Return409("Invalid Id. No record found with this Id.");
            }

            var storageSetting = _mapper.Map(request, entity);
            _documentStatusRepository.Update(storageSetting);
            if (await _uow.SaveAsync() <= -1)
            {
                return ServiceResponse<DocumentStatusDto>.Return500();
            }
            return ServiceResponse<DocumentStatusDto>.ReturnResultWith201(_mapper.Map<DocumentStatusDto>(entity));
        }
    }
}
