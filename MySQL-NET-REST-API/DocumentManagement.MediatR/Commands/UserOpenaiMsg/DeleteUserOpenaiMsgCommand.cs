using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class DeleteUserOpenaiMsgCommand : IRequest<ServiceResponse<bool>>
{
    public Guid Id { get; set; }
}
