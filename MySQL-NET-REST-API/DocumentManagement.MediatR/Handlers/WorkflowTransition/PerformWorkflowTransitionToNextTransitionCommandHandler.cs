using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class PerformWorkflowTransitionToNextTransitionCommandHandler(
    IWorkflowInstanceRepository workflowInstanceRepository,
    IWorkflowStepInstanceRepository workflowStepInstanceRepository,
    IWorkflowTransitionRepository workflowTransitionRepository,
    UserInfoToken userInfoToken,
    IWorkflowTransitionInstanceRepository workflowTransitionInstanceRepository,
    IUnitOfWork<DocumentContext> uow,
    IWorkflowStepRepository workflowStepRepository,
    IUserRoleRepository userRoleRepository,
    IWorkflowRepository workflowRepository,
    IDocumentRepository documentRepository,
    IUserNotificationRepository userNotificationRepository,
    IWorkflowInstanceEmailSenderRepository workflowInstanceEmailSenderRepository,
    IHubContext<UserHub, IHubClient> _hubContext
   )
    : IRequestHandler<PerformWorkflowTransitionToNextTransitionCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(PerformWorkflowTransitionToNextTransitionCommand request, CancellationToken cancellationToken)
    {
        var currentWorkflowInstace = await workflowInstanceRepository.FindBy(c => c.Id == request.WorkflowInstanceId).FirstOrDefaultAsync();
        var currentWorkflowStepInstance = await workflowStepInstanceRepository.FindBy(c => c.Id == request.WorkflowStepInstanceId).FirstOrDefaultAsync();
        var currentWorkflowTransition = await workflowTransitionRepository.FindBy(c => c.Id == request.TransitionId).FirstOrDefaultAsync();
        var workflow = await workflowRepository.FindBy(c => c.Id == currentWorkflowInstace.WorkflowId).FirstOrDefaultAsync();
        var document = await documentRepository.FindBy(c => c.Id == currentWorkflowInstace.DocumentId).FirstOrDefaultAsync();

        if (currentWorkflowInstace == null || currentWorkflowStepInstance == null || currentWorkflowTransition == null)
        {
            return ServiceResponse<bool>.Return404();
        }

        var nextWorkflowTransition = await workflowTransitionRepository.FindBy(c => c.FromStepId == currentWorkflowTransition.ToStepId).FirstOrDefaultAsync();


        if (currentWorkflowInstace != null)
        {
            if (nextWorkflowTransition != null)
            {
                currentWorkflowInstace.Status = Data.WorkflowInstanceStatus.InProgress;
            }
            else
            {
                currentWorkflowInstace.Status = Data.WorkflowInstanceStatus.Completed;
            }
            currentWorkflowInstace.UpdatedAt = DateTime.UtcNow;
            workflowInstanceRepository.Update(currentWorkflowInstace);
        }

        //Update current workflow step instance
        currentWorkflowStepInstance.Status = Data.WorkflowStepInstanceStatus.Completed;
        currentWorkflowStepInstance.UpdatedAt = DateTime.UtcNow;
        currentWorkflowStepInstance.CompletedAt = DateTime.UtcNow;
        workflowStepInstanceRepository.Update(currentWorkflowStepInstance);

        //Remove all email senders for this workflow step instance
        var workflowInstanceEmailSenders = workflowInstanceEmailSenderRepository.All.Where(c => c.WorkflowStepInstanceId == currentWorkflowStepInstance.Id).ToList();
        if (workflowInstanceEmailSenders.Count > 0)
        {
            workflowInstanceEmailSenderRepository.RemoveRange(workflowInstanceEmailSenders);
        }


        //Add new workflow step instance
        var workflowStepInstance = new WorkflowStepInstance()
        {
            Status = nextWorkflowTransition == null ? WorkflowStepInstanceStatus.Completed : WorkflowStepInstanceStatus.InProgress,
            WorkflowInstanceId = currentWorkflowInstace.Id,
            UserId = userInfoToken.Id,
            StepId = currentWorkflowTransition.ToStepId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            Comment = request.Comment
        };
        workflowStepInstanceRepository.Add(workflowStepInstance);
        //Add new workflow transition instance
        var workflowTransitionInstance = new Data.WorkflowTransitionInstance()
        {
            WorkflowInstanceId = currentWorkflowInstace.Id,
            WorkflowTransitionId = currentWorkflowTransition.Id,
            Status = WorkflowTransitionInstanceStatus.Completed,
            CreatedAt = currentWorkflowStepInstance.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
            Comment = request.Comment,
            PerformById = userInfoToken.Id
        };
        workflowTransitionInstanceRepository.Add(workflowTransitionInstance);

        List<NotificationWorkflowDto> lstNotificationWorkflow = new List<NotificationWorkflowDto>();
        if (workflowStepInstance.Status != WorkflowStepInstanceStatus.Completed)
        {
            var transitions = workflowTransitionRepository.All
                .Include(c => c.WorkflowTransitionUsers)
                .Include(c => c.WorkflowTransitionRoles)
                .Where(t => t.FromStepId == workflowStepInstance.StepId).ToList();

            foreach (var transition in transitions)
            {
                var toStep = workflowStepRepository.All.Where(c => c.Id == transition.ToStepId && c.WorkflowId == currentWorkflowInstace.WorkflowId).FirstOrDefault();
                if (transition.WorkflowTransitionRoles.Count > 0)
                {
                    var userIds = await userRoleRepository.GetUsersByRoles(transition.WorkflowTransitionRoles.Select(c => c.RoleId).ToList());
                    foreach (var userId in userIds)
                    {
                        lstNotificationWorkflow.Add(new NotificationWorkflowDto()
                        {
                            UserId = userId,
                            WorkflowInstanceId = currentWorkflowInstace.Id,
                            Message = $"You have been assigned the task of \"{transition.Name}\" the workflow \"{workflow.Name}\" for the document named \"{document.Name}.\"",
                            DocumentId = document.Id
                        });
                    }
                }
                if (transition.WorkflowTransitionUsers.Count > 0)
                {
                    foreach (var workflowUser in transition.WorkflowTransitionUsers)
                    {
                        if (lstNotificationWorkflow.Any(c => c.UserId == workflowUser.UserId))
                        {
                            continue;
                        }

                        lstNotificationWorkflow.Add(new NotificationWorkflowDto()
                        {
                            UserId = workflowUser.UserId,
                            WorkflowInstanceId = currentWorkflowInstace.Id,
                            Message = $"You have been assigned the task of \"{transition.Name}\" the workflow \"{workflow.Name}\" for the document named \"{document.Name}.\"",
                            DocumentId = document.Id
                        });
                    }
                }

            }
        }
        if (lstNotificationWorkflow.Count > 0)
        {
            userNotificationRepository.createWorkflowInstanceNotifications(lstNotificationWorkflow);
        }

        if (await uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.Return500();
        }
        try
        {
            if (lstNotificationWorkflow.Count > 0)
            {
                var users = lstNotificationWorkflow.Select(c => c.UserId).ToList();
                await userNotificationRepository.SendNotification(users);
            }

            await _hubContext.Clients.All.RefreshDocuments(document.CategoryId);
            await _hubContext.Clients.All.RefreshWorkflows();

        }
        catch (Exception)
        {

        }
        return ServiceResponse<bool>.ReturnResultWith201(true);

    }
}
