using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        public GetDocumentByIdQueryHandler(
           IDocumentRepository documentRepository,
             IMapper mapper
            )
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
        }

        public async Task<DocumentDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.FindAsync(request.Id);
            var documentDto = _mapper.Map<DocumentDto>(document);
            return documentDto;
        }
    }
}
