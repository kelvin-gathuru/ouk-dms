using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands;
public class GetCategoriesForListDocumentCommand : IRequest<List<CategoryDto>>
{
}
