
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class SendEmailReminderForWorkflowTransitionCommandHandler(
    IWorkflowStepRepository workflowStepRepository,
    IWorkflowStepInstanceRepository workflowStepInstanceRepository,
    IWorkflowTransitionRepository workflowTransitionRepository,
    IWorkflowInstanceEmailSenderRepository workflowInstanceEmailSenderRepository,
    IUnitOfWork<DocumentContext> uow,
    ISendEmailRepository sendEmailRepository,
    IUserRoleRepository userRoleRepository) : IRequestHandler<SendEmailReminderForWorkflowTransitionCommand, bool>
{
    public async Task<bool> Handle(SendEmailReminderForWorkflowTransitionCommand request, CancellationToken cancellationToken)
    {
        var currentDateTime = DateTime.UtcNow;
        var minDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
        var maxDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
        var sendEmailWorkflowStepInstances = workflowInstanceEmailSenderRepository.All
            .Where(c => c.CreatedAt >= minDate && c.CreatedAt <= maxDate)
            .AsNoTracking()
            .Select(c => c.WorkflowStepInstanceId)
            .ToList();
        var workflowStepInstances = await workflowStepInstanceRepository.All
            .Include(c => c.WorkflowInstance)
                .ThenInclude(c => c.Workflow)
            .Include(c => c.WorkflowInstance)
                .ThenInclude(c => c.Document)
            .Where(c => c.Status == WorkflowStepInstanceStatus.InProgress && (c.WorkflowInstance.Status == WorkflowInstanceStatus.InProgress || c.WorkflowInstance.Status == WorkflowInstanceStatus.Initiated)
            && !sendEmailWorkflowStepInstances.Contains(c.Id))
             .AsNoTracking()
            .ToListAsync();

        foreach (var workflowStepInstance in workflowStepInstances)
        {
            var workflowStep = await workflowStepRepository.All
                //.Include(c => c.WorkflowStepRoles)
                //.Include(c => c.WorkflowStepUsers)
                .Where(c => c.Id == workflowStepInstance.StepId).FirstOrDefaultAsync();
            if (workflowStep != null)
            {
                var transitions = await workflowTransitionRepository.All
                    .Include(c => c.WorkflowTransitionRoles)
                    .Include(c => c.WorkflowTransitionUsers)
                       .ThenInclude(c => c.User)
                    .Where(c => c.FromStepId == workflowStep.Id).ToListAsync();
                if (transitions.Count > 0)
                {
                    foreach (var transition in transitions)
                    {
                        var toWorkflowStep = await workflowStepRepository.All
                            //.Include(c => c.WorkflowStepRoles)
                            //.Include(c => c.WorkflowStepUsers)
                            //    .ThenInclude(c => c.User)
                            .Where(c => c.Id == transition.ToStepId).FirstOrDefaultAsync();

                        if (transition.Days > 0 || transition.Hours > 0 || transition.Minutes > 0)
                        {
                            var totalMinutesForTransition = GetTotalMinutesForTransition(transition.Days ?? 0, transition.Hours ?? 0, transition.Minutes ?? 0);
                            var totalMinutesForWorkflowInstanceStep = GetTotalMinutesForWorkflowInstanceStep(workflowStepInstance.CreatedAt);
                            if (totalMinutesForWorkflowInstanceStep >= totalMinutesForTransition)
                            {
                                List<User> users = new List<User>();
                                if (transition.WorkflowTransitionRoles != null && transition.WorkflowTransitionRoles.Count > 0)
                                {
                                    users = await userRoleRepository.GetUserDetailsByRoles(transition.WorkflowTransitionRoles.Select(c => c.RoleId).ToList());
                                }
                                if (transition.WorkflowTransitionUsers != null && transition.WorkflowTransitionUsers.Count > 0)
                                {
                                    users.AddRange(transition.WorkflowTransitionUsers.Select(c => c.User).ToList());
                                }

                                //Send Email
                                foreach (var user in users)
                                {
                                    sendEmailRepository.AddTransitionEmails(new SendEmail
                                    {
                                        Email = user.Email,
                                        FromEmail = user.Email,
                                        FromName = user.FirstName + ' ' + user.LastName,
                                        ToName = user.FirstName + ' ' + user.LastName,
                                        CreatedBy = user.Id,
                                        CreatedDate = DateTime.UtcNow,
                                    }, workflowStepInstance.WorkflowInstance.Document.Name, transition.Name, workflowStepInstance.WorkflowInstance.Workflow.Name);
                                }

                                workflowInstanceEmailSenderRepository.Add(new Data.Entities.WorkflowInstanceEmailSender
                                {
                                    Id = Guid.NewGuid(),
                                    CreatedAt = DateTime.UtcNow,
                                    WorkflowStepInstanceId = workflowStepInstance.Id,
                                    WorkflowTransitionId = transition.Id
                                });
                                await uow.SaveAsync();
                            }
                        }

                    }

                }
            }
        }

        return true;
    }

    private int GetTotalMinutesForTransition(int days, int hours, int minutes)
    {
        return (days * 24 * 60) + (hours * 60) + minutes;
    }

    private int GetTotalMinutesForWorkflowInstanceStep(DateTime createdDate)
    {
        //Date Difference in the minutes with the current date
        return (DateTime.UtcNow - createdDate).Minutes;
    }
}


