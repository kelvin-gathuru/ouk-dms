using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class AddDocumentCommentCommandHandler(IDocumentCommentRepository _documentCommentRepository,
IUnitOfWork<DocumentContext> _uow,
IMapper _mapper,
ILogger<AddDocumentCommentCommandHandler> _logger) : IRequestHandler<AddDocumentCommentCommand, ServiceResponse<DocumentCommentDto>>
{

    public async Task<ServiceResponse<DocumentCommentDto>> Handle(AddDocumentCommentCommand request, CancellationToken cancellationToken)
    {
        var documentCommentEntity = _mapper.Map<DocumentComment>(request);
        _documentCommentRepository.Add(documentCommentEntity);
        if (await _uow.SaveAsync() <= 0)
        {
            _logger.LogError("Error while adding industry");
            return ServiceResponse<DocumentCommentDto>.Return500();
        }
        var documentCommentDto = _mapper.Map<DocumentCommentDto>(documentCommentEntity);
        return ServiceResponse<DocumentCommentDto>.ReturnResultWith200(documentCommentDto);
    }
}

