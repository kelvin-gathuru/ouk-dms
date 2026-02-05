
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.WorkflowInstance
{
    public class GetAllWorkflowInstanceDocumentsCommandHandler(IWorkflowInstanceRepository workflowInstanceRepository) : IRequestHandler<GetAllWorkflowInstanceDocumentsCommand, List<DocumentShortDetail>>
    {
        public async Task<List<DocumentShortDetail>> Handle(GetAllWorkflowInstanceDocumentsCommand request, CancellationToken cancellationToken)
        {
            return await workflowInstanceRepository.All.Include(x => x.Document).IgnoreQueryFilters()
                .Select(x => new DocumentShortDetail
                {
                    Id = x.DocumentId,
                    Name = x.Document.Name,
                    DocumentNumber= x.Document.DocumentNumber
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
