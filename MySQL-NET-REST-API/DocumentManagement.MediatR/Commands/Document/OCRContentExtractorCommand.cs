
using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.MediatR.Commands;
public class OCRContentExtractorCommand : IRequest<ServiceResponse<string>>
{
    public string Extension { get; set; }
    [FromForm]
    public IFormFile File { get; set; }

}
