using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentTokenCommandHandler : IRequestHandler<GetDocumentTokenCommand, DocumentToken>
    {
        private readonly IDocumentTokenRepository _documentTokenRepository;


        public GetDocumentTokenCommandHandler(IDocumentTokenRepository documentTokenRepository
            )
        {
            _documentTokenRepository = documentTokenRepository;
        }

        public async Task<DocumentToken> Handle(GetDocumentTokenCommand request, CancellationToken cancellationToken)
        {
            var documentToken = await _documentTokenRepository.All.Where(c => c.Token == request.Token).FirstOrDefaultAsync();
            if (documentToken == null)
            {
                return null;
            }
            return documentToken;
        }
    }
}
