using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateUserClaimCommandHandler(
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    IUserClaimRepository _userClaimRepository,
    IHubContext<UserHub, IHubClient> _hubContext,
    IConnectionMappingRepository _connectionMappingRepository) : IRequestHandler<UpdateUserClaimCommand, UserClaimDto>
{
    public async Task<UserClaimDto> Handle(UpdateUserClaimCommand request, CancellationToken cancellationToken)
    {
        var appUserClaims = await _userClaimRepository.All.Where(c => c.UserId == request.Id).ToListAsync();
        var claimsToAdd = request.UserClaims.Where(c => !appUserClaims.Select(c => c.ClaimType).Contains(c.ClaimType)).ToList();
        claimsToAdd.ForEach(claim => claim.ClaimType = claim.ClaimType.Replace(" ", "_"));
        _userClaimRepository.AddRange(_mapper.Map<List<UserClaim>>(claimsToAdd));
        var claimsToDelete = appUserClaims.Where(c => !request.UserClaims.Select(cs => cs.ClaimType).Contains(c.ClaimType)).ToList();
        _userClaimRepository.RemoveRange(claimsToDelete);
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new UserClaimDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var currentUser = _connectionMappingRepository.GetUserInfoById(request.Id);
        if (currentUser != null)
        {
            await _hubContext.Clients.Client(currentUser.ConnectionId).UpdateUserPermission(request.Id);
        }
        return new UserClaimDto();
    }
}
