using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class CheckShareUserByCategoryCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid CategoryId { get; set; }
    }
}
