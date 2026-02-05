using System.Collections.Generic;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class GetAllDocumentMetaTagCommand : IRequest<ServiceResponse<List<DocumentMetaTagDto>>>
{
}
