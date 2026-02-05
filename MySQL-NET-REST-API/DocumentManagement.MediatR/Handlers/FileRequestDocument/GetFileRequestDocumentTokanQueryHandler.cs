using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetFileRequestDocumentTokanQueryHandler (IDocumentTokenRepository _documentTokenRepository, IUnitOfWork<DocumentContext> _uow): IRequestHandler<GetFileRequestDocumentTokenQuery, string>
    {
        public async Task<string> Handle(GetFileRequestDocumentTokenQuery request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid();
            var documentToken = await _documentTokenRepository.All.FirstOrDefaultAsync(c => c.DocumentId == request.Id);
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
