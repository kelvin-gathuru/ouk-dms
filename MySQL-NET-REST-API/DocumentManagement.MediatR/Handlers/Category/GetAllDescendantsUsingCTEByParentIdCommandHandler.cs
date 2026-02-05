using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class GetAllDescendantsUsingCTEByParentIdCommandHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetAllDescendantsUsingCTEByParentIdCommand, ServiceResponse<List<CategoryDto>>>
{
    public async Task<ServiceResponse<List<CategoryDto>>> Handle(GetAllDescendantsUsingCTEByParentIdCommand request, CancellationToken cancellationToken)
    {
        var categories = categoryRepository.GetAllDescendantsUsingCTEByParentId(request.ParentId);

        return ServiceResponse<List<CategoryDto>>.ReturnResultWith200(categories);
    }
}
