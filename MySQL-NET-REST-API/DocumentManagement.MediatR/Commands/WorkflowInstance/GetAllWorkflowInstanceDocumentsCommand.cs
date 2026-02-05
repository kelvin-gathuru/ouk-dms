using System.Collections.Generic;
using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetAllWorkflowInstanceDocumentsCommand: IRequest<List<DocumentShortDetail>>
    {
    }
}
