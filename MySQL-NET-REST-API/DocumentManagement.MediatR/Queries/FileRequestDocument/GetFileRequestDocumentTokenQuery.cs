using System;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetFileRequestDocumentTokenQuery : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
