using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers;
using MediatR;
using System;


namespace DocumentManagement.MediatR.Commands
{
    public class GetFileChunkCommand : IRequest<ServiceResponse<DocumentChunkDownload>>
    {
        public Guid DocumentVersionId { get; set; }
        public int ChunkIndex { get; set; }
    }
}
