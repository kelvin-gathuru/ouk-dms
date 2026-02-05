using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddDocumentTokenCommandHandler : IRequestHandler<AddDocumentTokenCommand, string>
    {
        private readonly IDocumentTokenRepository _documentTokenRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public AddDocumentTokenCommandHandler(IDocumentTokenRepository documentTokenRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _documentTokenRepository = documentTokenRepository;
            _uow = uow;
        }

        public async Task<string> Handle(AddDocumentTokenCommand request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid();
            var documentToken = _documentTokenRepository.Find(request.DocumentId);
            if (documentToken == null)
            {
                _documentTokenRepository.Add(new DocumentToken
                {
                    CreatedDate = DateTime.UtcNow,
                    DocumentId = request.DocumentId,
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
