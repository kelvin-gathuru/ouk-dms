using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateWorkflowInstanceCommandHandler(IWorkflowRepository _workflowRepository, IDocumentRepository _documentRepository, IWorkflowInstanceRepository _workflowInstanceRepository, IUnitOfWork<DocumentContext> _uow, IMapper _mapper) : IRequestHandler<UpdateWorkflowInstanceCommand, ServiceResponse<WorkflowInstanceDto>>
    {
        public async Task<ServiceResponse<WorkflowInstanceDto>> Handle(UpdateWorkflowInstanceCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _workflowInstanceRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if(entityExist == null)
            {
                return ServiceResponse<WorkflowInstanceDto>.Return404();
            }
            var workflowEntityExist = await _workflowRepository.FindBy(c => c.Id == request.WorkflowId).FirstOrDefaultAsync();
            if (workflowEntityExist == null)
            {
                return ServiceResponse<WorkflowInstanceDto>.Return404();
            }
            var DocumentEntityExist = await _documentRepository.FindBy(d => d.Id == request.DocumentId).FirstOrDefaultAsync();
            if (DocumentEntityExist == null)
            {
                return ServiceResponse<WorkflowInstanceDto>.Return404();
            }
            entityExist.Status = request.Status;
            entityExist.UpdatedAt = DateTime.UtcNow;
            _workflowInstanceRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<WorkflowInstanceDto>.Return500();
            }

            var entityDto = _mapper.Map<WorkflowInstanceDto>(entityExist);
            return ServiceResponse<WorkflowInstanceDto>.ReturnResultWith200(entityDto);
        }
    }
}
