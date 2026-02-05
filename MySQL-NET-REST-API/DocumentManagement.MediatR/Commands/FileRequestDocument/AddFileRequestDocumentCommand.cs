using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class AddFileRequestDocumentCommand : IRequest<ServiceResponse<List<FileRequestDocumentDto>>>
    {
        [FromForm]
        public List<IFormFile> Files { get; set; }
        public Guid FileRequestId { get; set; }
        public List<string> Names { get; set; }
    }
}
