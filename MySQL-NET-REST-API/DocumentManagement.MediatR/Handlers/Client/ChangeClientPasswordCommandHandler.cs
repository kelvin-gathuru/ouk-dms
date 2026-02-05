using DocumentManagement.Data;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ChangeClientPasswordCommandHandler : IRequestHandler<ChangeClientPasswordCommand, ServiceResponse<bool>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public ChangeClientPasswordCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _clientRepository = clientRepository;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(ChangeClientPasswordCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (client == null)
            {
                return ServiceResponse<bool>.ReturnFailed(404, "Client not found.");
            }

            var passwordHasher = new PasswordHasher<object>();
            var verifyResult = passwordHasher.VerifyHashedPassword(null, client.Password, request.OldPassword);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return ServiceResponse<bool>.ReturnFailed(400, "Incorrect old password.");
            }

            client.Password = passwordHasher.HashPassword(null, request.NewPassword);
            _clientRepository.Update(client);
            await _uow.SaveAsync();

            return ServiceResponse<bool>.ReturnResultWith200(true, "Password updated successfully.");
        }
    }
}
