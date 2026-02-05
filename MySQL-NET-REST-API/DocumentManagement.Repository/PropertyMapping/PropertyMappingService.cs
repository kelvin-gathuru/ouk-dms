using System;
using System.Collections.Generic;
using System.Linq;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.Repository;

public class PropertyMappingService : IPropertyMappingService
{
    private readonly Dictionary<string, PropertyMappingValue> _documentPropertyMapping =
       new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
       {
           { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
           { "DocumentNumber", new PropertyMappingValue(new List<string>() { "DocumentNumber" } )},
           { "Name", new PropertyMappingValue(new List<string>() { "Name" } )},
           { "Description", new PropertyMappingValue(new List<string>() { "Description" } )},
           { "CreatedBy", new PropertyMappingValue(new List<string>() { "User.FirstName" } )},
           { "ArchiveName", new PropertyMappingValue(new List<string>() { "ArchiveBy.FirstName" } )},
           { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
           { "CategoryName", new PropertyMappingValue(new List<string>() { "Category.Name" } )},
           { "DocumentStatus", new PropertyMappingValue(new List<string>() { "DocumentStatus.Name" } )},
           { "Client", new PropertyMappingValue(new List<string>() { "Client.CompanyName" } )},
           { "StorageType", new PropertyMappingValue(new List<string>() { "StorageType" } )},
           { "SignDate", new PropertyMappingValue(new List<string>() { "SignDate" } )},
           { "ExpiredDate", new PropertyMappingValue(new List<string>() { "ExpiredDate" } )}
       };

    private readonly Dictionary<string, PropertyMappingValue> _documentAuditTrailPropertyMapping =
      new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
      {
           { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                          { "Name", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
           { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
           { "DocumentNumber", new PropertyMappingValue(new List<string>() { "Document.DocumentNumber" } )},
           { "DocumentId", new PropertyMappingValue(new List<string>() { "DocumentId" } )},
           { "CategoryName", new PropertyMappingValue(new List<string>() { "Document.Category.Name" } )},
           { "CreatedBy", new PropertyMappingValue(new List<string>() { "CreatedByUser.FirstName" } )},
           { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
           { "OperationName", new PropertyMappingValue(new List<string>() { "OperationName" } )},
           { "PermissionUser", new PropertyMappingValue(new List<string>() { "AssignToUser.FirstName" } )},
           { "PermissionRole", new PropertyMappingValue(new List<string>() { "AssignToRole.Name" } )}
      };

    private readonly Dictionary<string, PropertyMappingValue> _notificationPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
            { "UserId", new PropertyMappingValue(new List<string>() { "UserId" } ) },
            { "Message", new PropertyMappingValue(new List<string>() { "Message" } ) },
            { "DocumentId", new PropertyMappingValue(new List<string>() { "DocumentId" } ) },
            { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
            { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
            { "IsRead", new PropertyMappingValue(new List<string>() { "IsRead" } )}
        };

    private readonly Dictionary<string, PropertyMappingValue> _loginAuditMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
            { "UserName", new PropertyMappingValue(new List<string>() { "UserName" } )},
            { "LoginTime", new PropertyMappingValue(new List<string>() { "LoginTime" } )},
            { "RemoteIP", new PropertyMappingValue(new List<string>() { "RemoteIP" } )},
            { "Status", new PropertyMappingValue(new List<string>() { "Status" } )},
            { "Provider", new PropertyMappingValue(new List<string>() { "Provider" } )}
        };

    private readonly Dictionary<string, PropertyMappingValue> _reminderMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
            { "Subject", new PropertyMappingValue(new List<string>() { "Subject" } )},
            { "Message", new PropertyMappingValue(new List<string>() { "Message" } )},
            { "Frequency", new PropertyMappingValue(new List<string>() { "Frequency" } )},
            { "DocumentName", new PropertyMappingValue(new List<string>() { "Document.Name" } )},
            { "DocumentNumber", new PropertyMappingValue(new List<string>() { "Document.DocumentNumber" } )},
            { "StartDate", new PropertyMappingValue(new List<string>() { "StartDate" },true )},
            { "EndDate", new PropertyMappingValue(new List<string>() { "EndDate" },true )},
            { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
            { "IsRepeated", new PropertyMappingValue(new List<string>() { "IsRepeated" } )},
            { "IsEmailNotification", new PropertyMappingValue(new List<string>() { "IsEmailNotification" } )},
            { "IsActive", new PropertyMappingValue(new List<string>() { "IsActive" } )}
        };

    private readonly Dictionary<string, PropertyMappingValue> _reminderSchedulerMapping =
       new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
       {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
            { "Subject", new PropertyMappingValue(new List<string>() { "Subject" } )},
            { "Message", new PropertyMappingValue(new List<string>() { "Message" } )},
            { "IsRead", new PropertyMappingValue(new List<string>() { "IsRead" } )},
            { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" }, true )}
       };

    private readonly Dictionary<string, PropertyMappingValue> _nLogMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
            { "MachineName", new PropertyMappingValue(new List<string>() { "MachineName" } )},
            { "Logged", new PropertyMappingValue(new List<string>() { "Logged" } )},
            { "Level", new PropertyMappingValue(new List<string>() { "Level" } )},
            { "Message", new PropertyMappingValue(new List<string>() { "Message" } )},
            { "Logger", new PropertyMappingValue(new List<string>() { "Logger" } )},
            { "Properties", new PropertyMappingValue(new List<string>() { "Properties" } )},
            { "Callsite", new PropertyMappingValue(new List<string>() { "Callsite" } )},
            { "Exception", new PropertyMappingValue(new List<string>() { "Exception" } )}
        };

    private readonly Dictionary<string, PropertyMappingValue> _userMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
            { "Email", new PropertyMappingValue(new List<string>() { "Email" } )},
            { "FirstName", new PropertyMappingValue(new List<string>() { "FirstName" } )},
            { "LastName", new PropertyMappingValue(new List<string>() { "LastName" } )},
            { "PhoneNumber", new PropertyMappingValue(new List<string>() { "PhoneNumber" })},
            { "IsDeleted", new PropertyMappingValue(new List<string>() { "IsDeleted" })}
        };

    private readonly Dictionary<string, PropertyMappingValue> _workflowInstanceMapping =
       new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
       {
            { "WorkflowId", new PropertyMappingValue(new[] { "Workflow.Id" }) },
            { "WorkflowName", new PropertyMappingValue(new[] { "Workflow.Name" }) },
            { "WorkflowInstanceId", new PropertyMappingValue(new[] { "Id" }) },
            { "WorkflowStepInstanceId", new PropertyMappingValue(new[] { "WorkflowStepInstances.Id" }) },
            { "WorkflowInstanceStatus", new PropertyMappingValue(new[] { "Status" }) },
            { "WorkflowStepId", new PropertyMappingValue(new[] { "WorkflowStepInstances.StepId" }) },
            { "WorkflowStepName", new PropertyMappingValue(new[] { "WorkflowStepInstances.WorkflowStep.StepName" }) },
            { "WorkflowStepInstanceStatus", new PropertyMappingValue(new[] { "WorkflowStepInstances.Status" }) },
            { "DocumentId", new PropertyMappingValue(new[] { "DocumentId" }) },
            { "DocumentName", new PropertyMappingValue(new[] { "Document.Name" }) },
            { "DocumentNumber", new PropertyMappingValue(new[] { "Document.DocumentNumber" }) },
            { "UpdatedAt", new PropertyMappingValue(new[] { "UpdatedAt" }, true ) },
       };

    private readonly Dictionary<string, PropertyMappingValue> _workflowTransitionLogMapping =
       new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
       {
            { "WorkflowInstanceId", new PropertyMappingValue(new[] { "Id" }) },
            { "WorkflowId", new PropertyMappingValue(new[] { "WorkflowInstance.Workflow.Id" }) },
            { "WorkflowName", new PropertyMappingValue(new[] { "WorkflowInstance.Workflow.Name" }) },
            { "DocumentId", new PropertyMappingValue(new[] { "WorkflowInstance.Document.Id" }) },
            { "DocumentNumber", new PropertyMappingValue(new[] { "WorkflowInstance.Document.DocumentNumber" }) },
            { "DocumentName", new PropertyMappingValue(new[] { "WorkflowInstance.Document.Name" }) },
            { "WorkflowInstanceStatus", new PropertyMappingValue(new[] { "WorkflowInstance.Status" }) },
            { "TransitionName", new PropertyMappingValue(new[] { "WorkflowTransition.Name" }) },
            { "InitiatedBy", new PropertyMappingValue(new[] { "WorkflowInstance.InitiatedBy" }) },
            { "Steps", new PropertyMappingValue(new[] { "WorkflowTransition.FromWorkflowStep" }) },
            { "WorkflowTransitionInstanceStatus", new PropertyMappingValue(new[] { "Status" }) },
            { "InititatedAt", new PropertyMappingValue(new[] { "WorkflowInstance.CreatedAt" }) },
            { "TransitionDate", new PropertyMappingValue(new[] { "UpdatedAt" }, true ) },
            { "PerformBy", new PropertyMappingValue(new[] { "PerformBy" }) },
            { "Comment", new PropertyMappingValue(new[] { "Comment" }) },

       };

    private readonly Dictionary<string, PropertyMappingValue> _userOpenaiMsgMapping =
       new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
       {
            { "Id", new PropertyMappingValue(new[] { "Id" }) },
            { "Title", new PropertyMappingValue(new List<string>() { "Title" } )},
            { "PromptInput", new PropertyMappingValue(new List<string>() { "PromptInput" } )},
            { "Language", new PropertyMappingValue(new List<string>() { "Language" } )},
            { "MaximumLength", new PropertyMappingValue(new List<string>() { "MaximumLength" } )},
            { "Creativity", new PropertyMappingValue(new List<string>() { "Creativity" } )},
            { "ToneOfVoice", new PropertyMappingValue(new List<string>() { "ToneOfVoice" } )},
            { "SelectedModel", new PropertyMappingValue(new List<string>() { "SelectedModel" } )},
            { "CreatedDate", new PropertyMappingValue(new List<string>() { "CreatedDate" } )},
       };

    private readonly IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();
    public PropertyMappingService()
    {
        propertyMappings.Add(new PropertyMapping<LoginAuditDto, LoginAudit>(_loginAuditMapping));
        propertyMappings.Add(new PropertyMapping<DocumentDto, Document>(_documentPropertyMapping));
        propertyMappings.Add(new PropertyMapping<DocumentAuditTrailDto, DocumentAuditTrail>(_documentAuditTrailPropertyMapping));
        propertyMappings.Add(new PropertyMapping<UserNotificationDto, UserNotification>(_notificationPropertyMapping));
        propertyMappings.Add(new PropertyMapping<ReminderDto, Reminder>(_reminderMapping));
        propertyMappings.Add(new PropertyMapping<ReminderSchedulerDto, ReminderScheduler>(_reminderSchedulerMapping));
        propertyMappings.Add(new PropertyMapping<NLogDto, NLog>(_nLogMapping));
        propertyMappings.Add(new PropertyMapping<UserDto, User>(_userMapping));
        propertyMappings.Add(new PropertyMapping<CurrentWorkflowDataDto, WorkflowInstance>(_workflowInstanceMapping));
        propertyMappings.Add(new PropertyMapping<WorkflowTransitionLogDto, WorkflowTransitionInstance>(_workflowTransitionLogMapping));
        propertyMappings.Add(new PropertyMapping<UserOpenaiMsgDto, UserOpenaiMsg>(_userOpenaiMsgMapping));

    }
    public Dictionary<string, PropertyMappingValue> GetPropertyMapping
        <TSource, TDestination>()
    {
        // get matching mapping
        var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Count() == 1)
        {
            return matchingMapping.First()._mappingDictionary;
        }

        throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
    }

    public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
    {
        var propertyMapping = GetPropertyMapping<TSource, TDestination>();

        if (string.IsNullOrWhiteSpace(fields))
        {
            return true;
        }

        // the string is separated by ",", so we split it.
        var fieldsAfterSplit = fields.Split(',');

        // run through the fields clauses
        foreach (var field in fieldsAfterSplit)
        {
            // trim
            var trimmedField = field.Trim();

            // remove everything after the first " " - if the fields 
            // are coming from an orderBy string, this part must be 
            // ignored
            var indexOfFirstSpace = trimmedField.IndexOf(" ");
            var propertyName = indexOfFirstSpace == -1 ?
                trimmedField : trimmedField.Remove(indexOfFirstSpace);

            // find the matching property
            if (!propertyMapping.ContainsKey(propertyName))
            {
                return false;
            }
        }
        return true;

    }

}
