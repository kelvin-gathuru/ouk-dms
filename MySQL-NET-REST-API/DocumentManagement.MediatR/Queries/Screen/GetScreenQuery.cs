using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetScreenQuery : IRequest<ServiceResponse<ScreenDto>>
    {
        public Guid Id { get; set; }
    }
}
