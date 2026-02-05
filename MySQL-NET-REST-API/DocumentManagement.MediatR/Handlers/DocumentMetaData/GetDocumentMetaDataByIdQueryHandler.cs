using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.DocumentMetaData;

public class GetDocumentMetaDataByIdQueryHandler : IRequestHandler<GetDocumentMetaDataByIdQuery, List<DocumentMetaDataDto>>
{
    private readonly IDocumentMetaDataRepository _documentMetaDataRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDocumentMetaDataByIdQueryHandler> _logger;
    public GetDocumentMetaDataByIdQueryHandler(
        IDocumentMetaDataRepository documentMetaDataRepository,
        IMapper mapper,
        ILogger<GetDocumentMetaDataByIdQueryHandler> logger)
    {
        _documentMetaDataRepository = documentMetaDataRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<DocumentMetaDataDto>> Handle(GetDocumentMetaDataByIdQuery request, CancellationToken cancellationToken)
    {
        var documentMetaData = await _documentMetaDataRepository.All
            .Include(c => c.DocumentMetaTag)
            .Where(c => c.DocumentId == request.DocumentId)
            .Select(c => new DocumentMetaDataDto
            {
                Id = c.Id,
                Metatag = c.Metatag,
                DocumentId = c.DocumentId,
                DocumentMetaTagId = c.DocumentMetaTagId,
                MetaTagDate = c.MetaTagDate,
                MetaTagType = c.DocumentMetaTag.Type
            })
            .ToListAsync();
        return documentMetaData;
    }
}
