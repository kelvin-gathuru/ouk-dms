using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddDocumentAuditTrailCommandHandler : IRequestHandler<AddDocumentAuditTrailCommand, DocumentAuditTrailDto>
    {
        private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfo;
        public AddDocumentAuditTrailCommandHandler(
           IDocumentAuditTrailRepository documentAuditTrailRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            UserInfoToken userInfo
            )
        {
            _documentAuditTrailRepository = documentAuditTrailRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfo = userInfo;
        }
        public async Task<DocumentAuditTrailDto> Handle(AddDocumentAuditTrailCommand request, CancellationToken cancellationToken)
        {
            var entity = new DocumentAuditTrail();
            entity.DocumentId = request.DocumentId;
            entity.CreatedBy = _userInfo.Id;
            entity.CreatedDate = DateTime.UtcNow;
            entity.OperationName = ParseEnum(request.OperationName);
            _documentAuditTrailRepository.Add(entity);
            if (await _uow.SaveAsync() <= -1)
            {
                var errorDto = new DocumentAuditTrailDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var entityDto = _mapper.Map<DocumentAuditTrailDto>(entity);
            return entityDto;
        }
        public DocumentOperation ParseEnum(string value)
        {
            return (DocumentOperation)Enum.Parse(typeof(DocumentOperation), value, true);
        }
    }
}
