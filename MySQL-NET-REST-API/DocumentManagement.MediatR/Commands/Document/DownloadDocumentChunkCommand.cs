using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers;
using MediatR;
using System;


namespace DocumentManagement.MediatR.Commands
{
    public class DownloadDocumentChunkCommand : IRequest<ServiceResponse<DocumentDownload>>
    {
        public Guid Id { get; set; }
        public Guid? DocumentVersionId { get; set; }
    }
}
