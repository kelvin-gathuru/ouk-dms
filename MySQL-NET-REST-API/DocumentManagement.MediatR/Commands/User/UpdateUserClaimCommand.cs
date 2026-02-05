using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateUserClaimCommand : IRequest<UserClaimDto>
    {
        public Guid Id { get; set; }
        public List<UserClaimDto> UserClaims { get; set; }
    }
}
