using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetTop10ReminderNotificationQueryHandler : IRequestHandler<GetTop10ReminderNotificationQuery, List<ReminderSchedulerDto>>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMapper _mapper;

        public GetTop10ReminderNotificationQueryHandler(
            IReminderSchedulerRepository reminderSchedulerRepository,
            UserInfoToken userInfoToken,
            IMapper mapper)
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _userInfoToken = userInfoToken;
            _mapper = mapper;
        }

        public async Task<List<ReminderSchedulerDto>> Handle(GetTop10ReminderNotificationQuery request, CancellationToken cancellationToken)
        {
            var reminderSchedulers = await _reminderSchedulerRepository.All.Where(c => !c.IsRead && c.UserId == _userInfoToken.Id)
                                    .OrderBy(c => c.CreatedDate)
                                    .Take(10)
                                    .ToListAsync();

            return _mapper.Map<List<ReminderSchedulerDto>>(reminderSchedulers);
        }
    }
}
