using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class GetUserOpenaiMsgResponseByIdCommand : IRequest<ServiceResponse<UserOpenaiMsgResponseDto>>
{
    public Guid Id { get; set; }
}
