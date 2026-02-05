using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetClientQuery : IRequest<ServiceResponse<ClientDto>>
    {
        public Guid Id { get; set; }
    }
}
