using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
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
public class AddPageActionCommand : IRequest<ServiceResponse<PageActionDto>>
{
    public string Name { get; set; }
    public Guid PageId { get; set; }
    public int Order { get; set; }
    public required string Code { get; set; }
}

public class AddPageActionCommandValidator : AbstractValidator<AddPageActionCommand>
{
    public AddPageActionCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
    }
}

public class AddPageActionCommandHandler(
    IPageActionRepository _actionRepository,
    IScreenRepository _pageRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    ILogger<AddPageActionCommandHandler> _logger) : IRequestHandler<AddPageActionCommand, ServiceResponse<PageActionDto>>
{
    public async Task<ServiceResponse<PageActionDto>> Handle(AddPageActionCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _actionRepository.FindBy(c => c.PageId == request.PageId && c.Name.Trim().ToLower() == request.Name.Trim().ToLower()).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            _logger.LogError("Action already exist.");
            return ServiceResponse<PageActionDto>.Return409("Action already exist.");
        }

        var page = await _pageRepository.FindAsync(request.PageId);
        if (page == null)
        {
            _logger.LogError("Page does not exists.");
            return ServiceResponse<PageActionDto>.Return404("Page does not exists.");
        }

        var entity = _mapper.Map<PageAction>(request);
        entity.Id = Guid.NewGuid();
        _actionRepository.Add(entity);

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<PageActionDto>.Return500();
        }

        var entityDto = _mapper.Map<PageActionDto>(entity);
        return ServiceResponse<PageActionDto>.ReturnResultWith200(entityDto);
    }
}
