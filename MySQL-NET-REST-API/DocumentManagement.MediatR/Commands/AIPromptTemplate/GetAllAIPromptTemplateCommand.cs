using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands;
public class GetAllAIPromptTemplateCommand : IRequest<ServiceResponse<List<AIPromptTemplate>>>
{

}
