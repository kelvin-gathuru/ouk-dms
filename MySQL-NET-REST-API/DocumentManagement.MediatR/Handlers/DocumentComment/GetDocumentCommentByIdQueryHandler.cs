using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class GetDocumentCommentByIdQueryHandler : IRequestHandler<GetDocumentCommentByIdQuery, List<DocumentCommentDto>>
{
    private readonly IDocumentCommentRepository _documentCommentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDocumentCommentByIdQueryHandler> _logger;
    public GetDocumentCommentByIdQueryHandler(
        IDocumentCommentRepository documentCommentRepository,
        IMapper mapper,
        ILogger<GetDocumentCommentByIdQueryHandler> logger)
    {
        _documentCommentRepository = documentCommentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<DocumentCommentDto>> Handle(GetDocumentCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var documentComment = await _documentCommentRepository.All
            .Include(c => c.CreatedByUser)
            .Where(c => c.DocumentId == request.DocumentId)
            .Select(c => new DocumentCommentDto
            {
                Id = c.Id,
                CreatedBy = $"{c.CreatedByUser.FirstName} {c.CreatedByUser.LastName}",
                CreatedDate = c.CreatedDate,
                Comment = c.Comment,
                DocumentId = c.DocumentId
            })
            .ToListAsync();
        return documentComment;
    }
}
