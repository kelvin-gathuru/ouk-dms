using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentSignatureCommand : IRequest<ServiceResponse<DocumentSignatureDto>>
    {
        public Guid DocumentId { get; set; }
        public string SignatureUrl { get; set; }
    }
}