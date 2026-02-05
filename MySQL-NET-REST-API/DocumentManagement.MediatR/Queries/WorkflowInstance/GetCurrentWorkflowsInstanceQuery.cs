using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetCurrentWorkflowsInstanceQuery: IRequest<ServiceResponse<List<CurrentWorkflowDataDto>>>
    {
    }
}

