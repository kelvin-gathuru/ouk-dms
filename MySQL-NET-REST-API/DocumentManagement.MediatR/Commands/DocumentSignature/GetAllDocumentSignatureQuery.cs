using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class GetAllDocumentSignatureQuery : IRequest<ServiceResponse<List<DocumentSignatureDataDto>>>
    {
        public Guid DocumentId { get; set; }
    }
}
