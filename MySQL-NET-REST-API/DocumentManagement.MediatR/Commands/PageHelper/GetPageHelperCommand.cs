using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class GetPageHelperCommand : IRequest<ServiceResponse<PageHelperDto>>
    {
        public Guid Id { get; set; }
    }
}
