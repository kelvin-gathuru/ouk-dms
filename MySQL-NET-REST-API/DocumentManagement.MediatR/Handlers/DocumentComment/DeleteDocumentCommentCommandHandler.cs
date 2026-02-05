using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteDocumentCommentCommandHandler : IRequestHandler<DeleteDocumentCommentCommand, ServiceResponse<bool>>
    {
        private readonly IDocumentCommentRepository _documentCommentRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteDocumentCommentCommandHandler> _logger;
        public DeleteDocumentCommentCommandHandler(
            IDocumentCommentRepository documentCommentRepository,
            IUnitOfWork<DocumentContext> uow,
            IMapper mapper,
            ILogger<DeleteDocumentCommentCommandHandler> logger)
        {
            _documentCommentRepository = documentCommentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteDocumentCommentCommand request, CancellationToken cancellationToken)
        {
            var documentCommentEntity = await _documentCommentRepository.FindAsync(request.Id);
            if (documentCommentEntity != null)
            {
                _documentCommentRepository.Remove(documentCommentEntity);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while adding industry");
                    return ServiceResponse<bool>.Return500();
                }
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            return ServiceResponse<bool>.Return404();
        }
    }
}
