using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserQueryHandler(
           IUserRepository userRepository,
            IMapper mapper
            )
        {

            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _userRepository.AllIncluding(c => c.UserRoles, cs => cs.UserClaims).FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entity != null)
                return _mapper.Map<UserDto>(entity);
            else
                return new UserDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "User not found" }
                };
        }
    }
}
