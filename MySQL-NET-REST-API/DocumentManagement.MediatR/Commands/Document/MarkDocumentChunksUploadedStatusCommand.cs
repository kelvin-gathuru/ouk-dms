
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class MarkDocumentChunksUploadedStatusCommand : IRequest<ServiceResponse<DocumentChunkStatus>>
    {
        public bool status { get; set; }
        public Guid DocumentId { get; set; }
    }
}
