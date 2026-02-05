using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class DeleteCategoryFromBackgroundServiceCommand : IRequest<bool>
{
    public Guid CategoryId { get; set; }
}
