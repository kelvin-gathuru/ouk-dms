using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ClientActivationCommandHandler : IRequestHandler<ClientActivationCommand, ServiceResponse<bool>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public ClientActivationCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _clientRepository = clientRepository;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(ClientActivationCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.FindBy(c => c.Email == request.Email).FirstOrDefaultAsync();
            if (client == null)
            {
                return ServiceResponse<bool>.ReturnFailed(404, "Client not found.");
            }

            if (client.IsActivated)
            {
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }

            if (client.ActivationCode != request.ActivationCode)
            {
                return ServiceResponse<bool>.ReturnFailed(400, "Invalid activation code.");
            }

            client.IsActivated = true;
            client.ActivationCode = null; // Clear code after activation
            _clientRepository.Update(client);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true, "Account activated successfully.");
        }
    }
}
