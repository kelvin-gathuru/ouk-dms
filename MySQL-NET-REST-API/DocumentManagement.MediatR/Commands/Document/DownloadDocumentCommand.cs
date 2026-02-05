using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DownloadDocumentCommand : IRequest<ServiceResponse<DocumentDownload>>
    {
        public Guid Id { get; set; }
        public bool IsVersion { get; set; }
    }
}
