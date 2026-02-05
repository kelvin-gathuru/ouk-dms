using Amazon.Runtime.Internal;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands
{
    public class DocumentChunksCommand : IRequest<ServiceResponse<List<DocumentChunkDto>>>
    {
        public Guid DocumentId { get; set; }
        public bool IsVersion { get; set; }
    }
   
}
