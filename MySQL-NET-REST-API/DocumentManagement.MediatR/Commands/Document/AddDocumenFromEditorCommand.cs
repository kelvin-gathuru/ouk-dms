using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.MediatR.Commands;
public class AddDocumenFromEditorCommand : IRequest<ServiceResponse<DocumentUrl>>
{
    [FromForm]
    public IFormFile Upload { get; set; }
}
