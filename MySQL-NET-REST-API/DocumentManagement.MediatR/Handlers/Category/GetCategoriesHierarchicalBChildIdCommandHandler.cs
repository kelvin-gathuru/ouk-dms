
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetCategoriesHierarchicalBChildIdCommandHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetCategoriesHierarchicalBChildIdCommand, ServiceResponse<List<CategoryDto>>>
    {
        public async Task<ServiceResponse<List<CategoryDto>>> Handle(GetCategoriesHierarchicalBChildIdCommand request, CancellationToken cancellationToken)
        {

            var hierarchy = new List<CategoryDto>();

            var category = await categoryRepository.FindAsync(request.ChildId);
            var level = 0;
            while (category != null)
            {
                var categoryDto = mapper.Map<CategoryDto>(category);
                categoryDto.Level = level;
                hierarchy.Add(categoryDto);
                level++;
                if (category.ParentId == null)
                    break;

                category = await categoryRepository.FindAsync(category.ParentId.Value);
            }
           
            return ServiceResponse<List<CategoryDto>>.ReturnResultWith200(hierarchy.OrderByDescending(c => c.Level).ToList());
        }
    }
}
