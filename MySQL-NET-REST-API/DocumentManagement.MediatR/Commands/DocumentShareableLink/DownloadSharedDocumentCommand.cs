using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class DownloadSharedDocumentCommand : IRequest<ServiceResponse<DocumentDownload>>
    {
        public string Code { get; set; }
        public string Password { get; set; }

    }
}
