using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.Repository;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands;

public class UpdatePageActionCommand : IRequest<ServiceResponse<PageActionDto>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid PageId { get; set; }
    public int Order { get; set; }
    public required string Code { get; set; }
}

public class UpdatePageActionCommandValidator : AbstractValidator<UpdatePageActionCommand>
{
    public UpdatePageActionCommandValidator()
    {
        RuleFor(c => c.Id).Must(NotEmptyGuid).WithMessage("Id is required");
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
    }

    private bool NotEmptyGuid(Guid p)
    {
        return p != Guid.Empty;
    }
}
public class UpdatePageActionCommandHandler(
    IPageActionRepository _actionRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    ILogger<UpdatePageActionCommandHandler> _logger) : IRequestHandler<UpdatePageActionCommand, ServiceResponse<PageActionDto>>
{
    public async Task<ServiceResponse<PageActionDto>> Handle(UpdatePageActionCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _actionRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id && c.PageId == request.PageId)
            .FirstOrDefaultAsync();
        if (entityExist != null)
        {
            _logger.LogError("Action Name Already Exist.");
            return ServiceResponse<PageActionDto>.Return409("Action Name Already Exist.");
        }
        entityExist = await _actionRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();

        if (entityExist == null)
        {
            _logger.LogError("Action Not Found.");
            return ServiceResponse<PageActionDto>.Return404("Action Not Found.");
        }

        entityExist.Name = request.Name;
        entityExist.Order = request.Order;
        entityExist.Code = request.Code;
        _actionRepository.Update(entityExist);

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<PageActionDto>.Return500();
        }

        var entityDto = _mapper.Map<PageActionDto>(entityExist);
        return ServiceResponse<PageActionDto>.ReturnResultWith200(entityDto);
    }
}
