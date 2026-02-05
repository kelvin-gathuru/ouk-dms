using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public SendEmailCommandHandler(
           ISendEmailRepository sendEmailRepository,
           UserInfoToken userInfoToken,
           IMapper mapper,
           IUnitOfWork<DocumentContext> uow
            )
        {
            _sendEmailRepository = sendEmailRepository;
            _userInfoToken = userInfoToken;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var sendEmail = _mapper.Map<SendEmail>(request);
            sendEmail.FromEmail = _userInfoToken.Email;
            sendEmail.IsSend = false;
            _sendEmailRepository.Add(sendEmail);
            if (await _uow.SaveAsync() <= -1)
            {
                return false;
            }
            return true;
        }
    }
}
