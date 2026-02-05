using System;
using DocumentManagement.Helper;
using MediatR;
namespace DocumentManagement.MediatR.Commands;
public class GetUserOpenaiMsgByIdQuery : IRequest<ServiceResponse<bool>>
{
    public Guid Id { get; set; }
}
