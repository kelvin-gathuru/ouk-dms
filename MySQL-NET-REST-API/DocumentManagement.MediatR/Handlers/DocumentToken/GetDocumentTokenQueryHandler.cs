using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumentTokenQueryHandler : IRequestHandler<GetDocumentTokenQuery, string>
    {
        private readonly IDocumentTokenRepository _documentTokenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IDocumentVersionRepository _documentVersionRepository;

        public GetDocumentTokenQueryHandler(IDocumentTokenRepository documentTokenRepository,
            IUnitOfWork<DocumentContext> uow,
            IDocumentVersionRepository documentVersionRepository)
        {
            _documentTokenRepository = documentTokenRepository;
            _uow = uow;
            _documentVersionRepository= documentVersionRepository;
        }
        public async Task<string> Handle(GetDocumentTokenQuery request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid();
            var documentVersion = _documentVersionRepository.All.FirstOrDefault(c => (c.DocumentId == request.Id && c.IsCurrentVersion) || c.Id == request.Id);
            var documentToken = await _documentTokenRepository.All.FirstOrDefaultAsync(c => c.DocumentId == documentVersion.DocumentId);
            if (documentToken == null)
            {
                _documentTokenRepository.Add(new DocumentToken
                {
                    CreatedDate = DateTime.UtcNow,
                    DocumentId = request.Id,
                    Token = token
                });
                await _uow.SaveAsync();
            }
            else
            {
                token = documentToken.Token;
            }
            return token.ToString();
        }
    }
}
