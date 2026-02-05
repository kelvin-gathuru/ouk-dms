using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

using Newtonsoft.Json;

namespace DocumentManagement.MediatR.Commands
{
    public class ChangeClientPasswordCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; }
        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}
