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

namespace DocumentManagement.MediatR.Handlers;

public class UpdateAllowFileExtensionCommandHandler(IAllowFileExtensionRepository _allowFileExtensionRepository, IUnitOfWork<DocumentContext> _uow, IMapper _mapper) : IRequestHandler<UpdateAllowFileExtensionCommand, ServiceResponse<AllowFileExtensionDto>>
{
    public async Task<ServiceResponse<AllowFileExtensionDto>> Handle(UpdateAllowFileExtensionCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _allowFileExtensionRepository.All
             .FirstOrDefaultAsync(c => c.FileType == request.FileType && c.Id != request.Id);
        if (entityExist != null)
        {
            return ServiceResponse<AllowFileExtensionDto>.Return409("File type with same name already exists.");
        }

        entityExist = await _allowFileExtensionRepository.All
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (entityExist == null)
        {
            return ServiceResponse<AllowFileExtensionDto>.Return409("File type does not exists.");
        }

        entityExist.FileType = request.FileType;
        entityExist.Extension = request.Extension;
        _allowFileExtensionRepository.Update(entityExist);

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<AllowFileExtensionDto>.Return500();
        }

        var entityDto = _mapper.Map<AllowFileExtensionDto>(entityExist);
        return ServiceResponse<AllowFileExtensionDto>.ReturnResultWith200(entityDto);
    }
}
