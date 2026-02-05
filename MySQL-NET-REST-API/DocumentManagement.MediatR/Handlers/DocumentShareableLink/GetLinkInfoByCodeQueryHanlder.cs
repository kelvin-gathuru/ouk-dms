using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetLinkInfoByCodeQueryHanlder : IRequestHandler<GetLinkInfoByCodeQuery, ServiceResponse<DocumentShareableLinkDto>>
    {
        private readonly IDocumentShareableLinkRepository _documentShareableLinkRepository;
        private readonly IMapper _mapper;

        public GetLinkInfoByCodeQueryHanlder(IDocumentShareableLinkRepository documentShareableLinkRepository,
            IMapper mapper)
        {
            _documentShareableLinkRepository = documentShareableLinkRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<DocumentShareableLinkDto>> Handle(GetLinkInfoByCodeQuery request, CancellationToken cancellationToken)
        {
            var link = await _documentShareableLinkRepository.AllIncluding(c => c.Document)
                .FirstOrDefaultAsync(c => c.LinkCode == request.Code);
            if (link == null)
            {
                return ServiceResponse<DocumentShareableLinkDto>.Return404();
            }

            var linkInfo = _mapper.Map<DocumentShareableLinkDto>(link);
            linkInfo.DocumentName = link.Document?.Name;
            linkInfo.Url = link.Document?.Url;

            if (!string.IsNullOrWhiteSpace(link.Password))
            {
                linkInfo.HasPassword = true;
                var base64EncodedBytes = Convert.FromBase64String(linkInfo.Password);
                string existingPassowrd = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                linkInfo.Password = existingPassowrd;
            }

            if (link.LinkExpiryTime.HasValue)
            {
                linkInfo.IsLinkExpired = DateTime.UtcNow > link.LinkExpiryTime;
            }

            return ServiceResponse<DocumentShareableLinkDto>.ReturnResultWith200(linkInfo);
        }
    }
}
