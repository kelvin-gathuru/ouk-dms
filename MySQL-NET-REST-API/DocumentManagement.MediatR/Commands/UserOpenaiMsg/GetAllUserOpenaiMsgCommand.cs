using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class GetAllUserOpenaiMsgCommand : IRequest<UserOpenaiMsgList>
{
    public UserOpenaiMsgResource userOpenaiMsgResource { get; set; }
}
