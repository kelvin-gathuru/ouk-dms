using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class GetAllUserOpenaiMsgCommandHandler(IUserOpenaiMsgRepository _userOpenaiMsgRepository) : IRequestHandler<GetAllUserOpenaiMsgCommand, UserOpenaiMsgList>
{
    public async Task<UserOpenaiMsgList> Handle(GetAllUserOpenaiMsgCommand request, CancellationToken cancellationToken)
    {
        return await _userOpenaiMsgRepository.GetUserOpenaiMsgAsync(request.userOpenaiMsgResource);
    }
}
