using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands;
public class DeletePageActionCommand : IRequest<ServiceResponse<PageActionDto>>
{
    public Guid Id { get; set; }
}

public class DeletePageActionCommandHandler(
      IPageActionRepository actionRepository,
      IUnitOfWork<DocumentContext> uow,
      ILogger<DeletePageActionCommandHandler> logger
      ) : IRequestHandler<DeletePageActionCommand, ServiceResponse<PageActionDto>>
{
    public async Task<ServiceResponse<PageActionDto>> Handle(DeletePageActionCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await actionRepository.FindAsync(request.Id);
        if (entityExist == null)
        {
            logger.LogError("Not found");
            return ServiceResponse<PageActionDto>.Return404();
        }

        actionRepository.Delete(request.Id);

        if (await uow.SaveAsync() <= 0)
        {
            return ServiceResponse<PageActionDto>.Return500();
        }

        return ServiceResponse<PageActionDto>.ReturnSuccess();
    }
}