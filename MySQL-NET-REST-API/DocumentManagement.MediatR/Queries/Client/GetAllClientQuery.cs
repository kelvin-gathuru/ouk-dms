using System.Collections.Generic;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllClientQuery : IRequest<ServiceResponse<List<ClientDto>>>
    {
    }
}
