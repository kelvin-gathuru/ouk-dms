using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class GetLogQuery : IRequest<ServiceResponse<NLogDto>>
    {
        public Guid Id { get; set; }
    }
}
