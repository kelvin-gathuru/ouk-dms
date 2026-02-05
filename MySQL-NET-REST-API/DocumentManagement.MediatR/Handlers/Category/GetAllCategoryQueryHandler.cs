using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;

public class GetAllCategoryQueryHandler(ICategoryRepository _categoryRepository, IMapper _mapper) : IRequestHandler<GetAllCategoryQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        //var categories = await _categoryRepository.All
        //   .Include(c => c.Children)
        //   .Where(c => c.ParentId == null)
        //   .ToListAsync();

        var categories = _categoryRepository.GetAllDescendantsUsingCTE().Where(c => !c.IsArchive).ToList();
        return _mapper.Map<List<CategoryDto>>(categories);
    }
}
