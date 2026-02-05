using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocumentManagement.MediatR.Commands
{
    public class UploadDocumentChunkCommand : IRequest<ServiceResponse<DocumentChunkDto>>
    {
        [FromForm]
        public IFormFile File { get; set; }
        public int ChunkIndex { get; set; }
        public long Size { get; set; }
        public int TotalChunks { get; set; }
        public string Extension { get; set; }
        public Guid DocumentVersionId { get; set; }
  
    }
}
