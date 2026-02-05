using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetSubCategoriesQueryHandler (ICategoryRepository _categoryRepository, IMapper _mapper) : IRequestHandler<GetSubCategoriesQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            var entity = await _categoryRepository.All.Where(c => c.ParentId == request.Id).ToListAsync();
            return _mapper.Map<List<CategoryDto>>(entity);
        }
    }
}
