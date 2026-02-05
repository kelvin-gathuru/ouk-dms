using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class DocumentSignByUserCommandHandler(IDocumentRepository documentRepository, UserInfoToken userInfoToken) : IRequestHandler<DocumentSignByUserCommand, bool>
    {
        public async Task<bool> Handle(DocumentSignByUserCommand request, CancellationToken cancellationToken)
        {
            return await documentRepository.All.AnyAsync(d => d.Id == request.DocumentId && d.SignById == userInfoToken.Id);
        }
    }
}
