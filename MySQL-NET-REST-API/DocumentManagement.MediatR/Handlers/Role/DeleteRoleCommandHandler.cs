using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, RoleDto>
    {
        private readonly RoleManager<Role> _roleManger;
        private readonly IUserRoleRepository _userRoleRepository;
        public DeleteRoleCommandHandler(
            RoleManager<Role> roleManger,
            IUserRoleRepository userRoleRepository
            )
        {
            _roleManger = roleManger;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<RoleDto> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _roleManger.FindByIdAsync(request.Id.ToString());
            if (entityExist == null)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found Role" }
                };
                return errorDto;
            }
            entityExist.IsDeleted = true;
            IdentityResult result = await _roleManger.UpdateAsync(entityExist);
            
            await _userRoleRepository.All.Where(c => c.RoleId == request.Id).ExecuteDeleteAsync();

            if (!result.Succeeded)
            {
                var errorDto = new RoleDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            return new RoleDto
            {
                StatusCode = 200,
                Messages = new List<string> { "Role deleted successfully." }
            };
        }
    }
}
