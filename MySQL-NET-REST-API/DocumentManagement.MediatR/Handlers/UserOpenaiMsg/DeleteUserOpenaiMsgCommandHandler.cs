using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class DeleteUserOpenaiMsgCommandHandler(
    IUserOpenaiMsgRepository _userOpenaiMsgRepository,
    IUnitOfWork<DocumentContext> _uow
    ) : IRequestHandler<DeleteUserOpenaiMsgCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(DeleteUserOpenaiMsgCommand request, CancellationToken cancellationToken)
    {
        var userOpenaiMsg = await _userOpenaiMsgRepository.FindAsync(request.Id);
        if (userOpenaiMsg == null)
        {
            return ServiceResponse<bool>.Return404();
        }
        _userOpenaiMsgRepository.Remove(userOpenaiMsg);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.Return500();
        }
        return ServiceResponse<bool>.ReturnResultWith200(true);
    }
}
