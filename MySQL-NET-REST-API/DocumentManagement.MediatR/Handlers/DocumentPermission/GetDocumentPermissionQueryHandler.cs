using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.DocumentPermission.User
{
    public class GetDocumentPermissionQueryHandler
         : IRequestHandler<GetDocumentPermissionQuery, List<DocumentPermissionDto>>
    {
        private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
        private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
        private readonly IMapper _mapper;
        public GetDocumentPermissionQueryHandler(
            IDocumentRolePermissionRepository documentRolePermissionRepository,
            IMapper mapper,
            IDocumentUserPermissionRepository documentUserPermissionRepository)
        {
            _documentRolePermissionRepository = documentRolePermissionRepository;
            _mapper = mapper;
            _documentUserPermissionRepository = documentUserPermissionRepository;
        }
        public async Task<List<DocumentPermissionDto>> Handle(GetDocumentPermissionQuery request, CancellationToken cancellationToken)
        {
            var result = new List<DocumentPermissionDto>();
            var documentRolePermissions = await _documentRolePermissionRepository
                .AllIncluding(c => c.Role)
                .Where(c => c.DocumentId == request.DocumentId)
                .ToListAsync();
            var rolePermissions = _mapper.Map<List<DocumentPermissionDto>>(documentRolePermissions);
            rolePermissions.ForEach(p => p.Type = "Role");
            result.AddRange(rolePermissions);

            var documentUserPermissions = await _documentUserPermissionRepository
                .AllIncluding(c => c.User)
                .Where(c => c.DocumentId == request.DocumentId)
                .ToListAsync();
            var userPermissions = _mapper.Map<List<DocumentPermissionDto>>(documentUserPermissions);
            userPermissions.ForEach(p => p.Type = "User");
            result.AddRange(userPermissions);
            return result;
        }
    }
}
