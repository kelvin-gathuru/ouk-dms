using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllScreenQueryHandler : IRequestHandler<GetAllScreenQuery, List<ScreenDto>>
    {
        private readonly IScreenRepository _screenRepository;
        private readonly IMapper _mapper;

        public GetAllScreenQueryHandler(
            IScreenRepository screenRepository,
            IMapper mapper)
        {
            _screenRepository = screenRepository;
            _mapper = mapper;

        }
        public async Task<List<ScreenDto>> Handle(GetAllScreenQuery request, CancellationToken cancellationToken)
        {
            var entities = await _screenRepository.All.OrderBy(c => c.OrderNo).ToListAsync();
            return _mapper.Map<List<ScreenDto>>(entities);
        }
    }
}
