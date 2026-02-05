using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands;
public class GetAllPageActionQuery : IRequest<List<PageActionDto>>
{
}

public class GetAllPageActionQueryHandler(
       IPageActionRepository actionRepository,
       IMapper mapper) : IRequestHandler<GetAllPageActionQuery, List<PageActionDto>>
{
    public async Task<List<PageActionDto>> Handle(GetAllPageActionQuery request, CancellationToken cancellationToken)
    {
        var entities = await actionRepository.All.OrderBy(c => c.Order).ToListAsync();
        return mapper.Map<List<PageActionDto>>(entities);
    }
}


