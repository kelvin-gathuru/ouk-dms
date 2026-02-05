using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllFileRequestQuery : IRequest<ServiceResponse<List<FileRequestListDataDto>>>
    {
    }
}
