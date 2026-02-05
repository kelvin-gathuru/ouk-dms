using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddReminderCommandHandler
    : IRequestHandler<AddReminderCommand, ServiceResponse<ReminderDto>>
{

    private readonly IReminderRepository _reminderRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly UserInfoToken _userInfoToken;
    private readonly IMediator _mediator;

    public AddReminderCommandHandler(IReminderRepository reminderRepository,
        IMapper mapper,
        IUnitOfWork<DocumentContext> uow,
        UserInfoToken userInfoToken,
        IMediator mediator)
    {
        _reminderRepository = reminderRepository;
        _mapper = mapper;
        _uow = uow;
        _userInfoToken = userInfoToken;
        _mediator = mediator;
    }

    public async Task<ServiceResponse<ReminderDto>> Handle(AddReminderCommand request, CancellationToken cancellationToken)
    {
        if (!request.Frequency.HasValue)
        {
            request.Frequency = Frequency.OneTime;
        }
        if (!request.IsRepeated)
        {
            request.Frequency = Frequency.OneTime;
        }

        if (!request.ReminderUsers.Any(c => c.UserId == _userInfoToken.Id))
        {
            request.ReminderUsers.Add(new ReminderUserDto
            {
                UserId = _userInfoToken.Id
            });
        }
        var reminder = _mapper.Map<Reminder>(request);
        _reminderRepository.Add(reminder);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<ReminderDto>.Return500();
        }
        if (reminder.Frequency == Frequency.OneTime && !reminder.IsRepeated)
        {
            var customDateReminderServicesQuery = new CustomDateReminderServicesQuery();
            await _mediator.Send(customDateReminderServicesQuery, cancellationToken);
        }

        return ServiceResponse<ReminderDto>.ReturnResultWith201(_mapper.Map<ReminderDto>(reminder));
    }
}
