using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands;

public class GetPageActionsByPageQuery : IRequest<List<PageActionDto>>
{
    public Guid PageId { get; set; }
}
public class GetPageActionQueryHandlerHandler(
    IPageActionRepository _pageActionRepository,
    IMapper _mapper,
    ILogger<GetPageActionQueryHandlerHandler> _logger
    ) : IRequestHandler<GetPageActionsByPageQuery, List<PageActionDto>>
{
    public async Task<List<PageActionDto>> Handle(GetPageActionsByPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _pageActionRepository.All.Where(d => d.PageId == request.PageId).ToListAsync();
            return _mapper.Map<List<PageActionDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting actions by page ID: {PageId}", request.PageId);
            return [];
        }
    }
}