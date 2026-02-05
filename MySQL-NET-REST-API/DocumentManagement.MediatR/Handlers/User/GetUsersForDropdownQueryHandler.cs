using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetUsersForDropdownQueryHandler(
        IUserRepository userRepository) : IRequestHandler<GetUsersForDropdownQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(GetUsersForDropdownQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.All.Select(c => new UserDto
            {
                Id = c.Id,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                UserName = c.UserName
            }).ToListAsync();

            return users;
        }
    }
}
