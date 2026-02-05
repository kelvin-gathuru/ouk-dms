using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateFileRequestDocumentCommand : IRequest<ServiceResponse<FileRequestDocumentDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Reason { get; set; }
    }
}
