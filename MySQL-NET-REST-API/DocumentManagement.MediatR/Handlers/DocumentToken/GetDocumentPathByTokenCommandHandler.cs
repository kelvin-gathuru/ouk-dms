using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace YourDrive.MediatR.Handlers
{
    public class GetDocumentPathByTokenCommandHandler : IRequestHandler<GetDocumentPathByTokenCommand, bool>
    {
        private readonly IDocumentTokenRepository _documentTokenRepository;
        private readonly IDocumentShareableLinkRepository _documentShareableLinkRepository;


        public GetDocumentPathByTokenCommandHandler(IDocumentTokenRepository documentTokenRepository,
            IDocumentShareableLinkRepository documentShareableLinkRepository)
        {
            _documentTokenRepository = documentTokenRepository;
            _documentShareableLinkRepository = documentShareableLinkRepository;
        }

        public async Task<bool> Handle(GetDocumentPathByTokenCommand request, CancellationToken cancellationToken)
        {
            var documentId = request.Id;
            if (request.IsPublic)
            {
                var link = await _documentShareableLinkRepository.All.FirstOrDefaultAsync(c => c.LinkCode == request.Id.ToString());
                if (link == null)
                {
                    return false;
                }
                documentId = link.DocumentId;
            }

            if (await _documentTokenRepository.All.AnyAsync(c => c.DocumentId == documentId && c.Token == request.Token))
            {
                return true;
            }

            return false;
        }
    }
}
