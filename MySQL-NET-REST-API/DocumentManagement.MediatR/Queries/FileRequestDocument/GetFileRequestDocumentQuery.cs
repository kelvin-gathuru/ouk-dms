using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetFileRequestDocumentQuery : IRequest<ServiceResponse<FileRequestDocumentDto>>
    {
        public Guid Id { get; set; }
    }
}
