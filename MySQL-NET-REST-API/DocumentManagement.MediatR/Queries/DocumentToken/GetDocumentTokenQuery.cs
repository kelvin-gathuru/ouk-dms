using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentTokenQuery : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
