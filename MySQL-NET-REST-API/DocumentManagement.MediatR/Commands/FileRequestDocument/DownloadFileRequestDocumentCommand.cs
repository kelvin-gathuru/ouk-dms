using System;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class DownloadFileRequestDocumentCommand : IRequest<ServiceResponse<DocumentDownload>>
    {
        public Guid Id { get; set; }
    }
}
