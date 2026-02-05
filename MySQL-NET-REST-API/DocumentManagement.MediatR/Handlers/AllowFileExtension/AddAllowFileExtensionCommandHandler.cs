using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddAllowFileExtensionCommandHandler(
    IAllowFileExtensionRepository _allowFileExtensionRepository,
    IMapper _mapper,
    IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddAllowFileExtensionCommand, ServiceResponse<AllowFileExtensionDto>>
{

    public async Task<ServiceResponse<AllowFileExtensionDto>> Handle(AddAllowFileExtensionCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _allowFileExtensionRepository.FindBy(c => c.FileType == request.FileType).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            return ServiceResponse<AllowFileExtensionDto>.Return409("File type with same name already exists.");
        }

        var entity = _mapper.Map<AllowFileExtension>(request);
        entity.Id = Guid.NewGuid();
        _allowFileExtensionRepository.Add(entity);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<AllowFileExtensionDto>.Return500();
        }
        var entityDto = _mapper.Map<AllowFileExtensionDto>(entity);
        return ServiceResponse<AllowFileExtensionDto>.ReturnResultWith201(entityDto);
    }
}
