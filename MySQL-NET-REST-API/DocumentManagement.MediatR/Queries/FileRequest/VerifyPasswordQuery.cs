using DocumentManagement.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class VerifyPasswordQuery : IRequest<ServiceResponse<bool>>
    {
        [FromRoute]
        public Guid Id { get; set; }
        [FromQuery]
        public string Password { get; set; }
    }
}
