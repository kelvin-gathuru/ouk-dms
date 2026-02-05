using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System.Collections.Generic;


namespace DocumentManagement.MediatR.Commands
{
    public class DeepSearchCommand:  IRequest<ServiceResponse<List<DocumentDto>>>
    {
        public string SearchQuery { get; set; }
    }
}
