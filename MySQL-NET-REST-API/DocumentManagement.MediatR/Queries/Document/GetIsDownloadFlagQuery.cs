using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetIsDownloadFlagQuery : IRequest<bool>
    {
        public Guid DocumentId { get; set; }
    }
}
