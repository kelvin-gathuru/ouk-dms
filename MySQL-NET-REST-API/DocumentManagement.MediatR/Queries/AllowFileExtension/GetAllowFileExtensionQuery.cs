using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllowFileExtensionQuery : IRequest<ServiceResponse<AllowFileExtensionDto>>
    {
        public Guid Id { get; set; }
    }
}
