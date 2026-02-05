using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllScreenQuery : IRequest<List<ScreenDto>>
    {
    }
}
