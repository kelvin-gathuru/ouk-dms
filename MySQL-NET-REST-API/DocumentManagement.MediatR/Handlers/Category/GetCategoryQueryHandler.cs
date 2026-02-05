using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetCategoryQueryHandler (ICategoryRepository _categoryRepository, IMapper _mapper) : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _categoryRepository.FindAsync(request.Id);
            if (entity != null)
                return _mapper.Map<CategoryDto>(entity);
            else
                return new CategoryDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Category is not found." }
                };
        }
    }
}
