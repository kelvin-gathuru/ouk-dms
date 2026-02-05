using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class UploadNewDocumentVersionCommand : IRequest<ServiceResponse<DocumentVersionDto>>
    {
        public Guid DocumentId { get; set; }
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public bool IsSignatureExists { get; set; }
        public string Extension { get; set; }
        public string Comment { get; set; }
    }
}
