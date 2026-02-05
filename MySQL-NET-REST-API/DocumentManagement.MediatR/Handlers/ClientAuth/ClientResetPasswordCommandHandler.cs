using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
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
    public class ClientResetPasswordCommandHandler : IRequestHandler<ClientResetPasswordCommand, ServiceResponse<bool>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public ClientResetPasswordCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _clientRepository = clientRepository;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(ClientResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.FindBy(c => c.Email == request.Email).FirstOrDefaultAsync();
            if (client == null)
            {
                return ServiceResponse<bool>.ReturnFailed(404, "Client not found.");
            }

            if (client.ActivationCode != request.Token)
            {
                return ServiceResponse<bool>.ReturnFailed(400, "Invalid reset token.");
            }

            var passwordHasher = new PasswordHasher<object>();
            client.Password = passwordHasher.HashPassword(null, request.NewPassword);
            client.ActivationCode = null; // Clear token
            // Ensure client is activated if they reset password? Maybe.
            // client.IsActivated = true; 

            _clientRepository.Update(client);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true, "Password reset successfully.");
        }
    }
}
