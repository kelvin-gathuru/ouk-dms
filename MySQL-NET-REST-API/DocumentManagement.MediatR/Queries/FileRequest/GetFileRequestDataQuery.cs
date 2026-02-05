using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetFileRequestDataQuery : IRequest<ServiceResponse<FileRequestDto>>
    {
        public Guid Id { get; set; }
    }
}
