using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteScreenCommand : IRequest<ScreenDto>
    {
        public Guid Id { get; set; }
    }
}
