using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetScreenOperationQuery : IRequest<ScreenOperationDto>
    {
        public Guid Id { get; set; }
    }
}
