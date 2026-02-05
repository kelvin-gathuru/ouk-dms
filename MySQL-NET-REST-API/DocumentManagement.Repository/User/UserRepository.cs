using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DocumentManagement.Repository;

public class UserRepository : GenericRepository<User, DocumentContext>,
      IUserRepository
{
    private readonly JwtSettings _settings = null;
    private readonly IUserClaimRepository _userClaimRepository;
    private readonly IRoleClaimRepository _roleClaimRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly ICompanyProfileRepository _companyProfileRepository;
    private readonly IPageActionRepository _pageActionRepository;

    public UserRepository(
        IUnitOfWork<DocumentContext> uow,
         JwtSettings settings,
         IUserClaimRepository userClaimRepository,
         IRoleClaimRepository roleClaimRepository,
         IUserRoleRepository userRoleRepository,
         IPageActionRepository pageActionRepository,

         IPropertyMappingService propertyMappingService,
         ICompanyProfileRepository companyProfileRepository
        ) : base(uow)
    {
        _roleClaimRepository = roleClaimRepository;
        _userClaimRepository = userClaimRepository;
        _userRoleRepository = userRoleRepository;
        _settings = settings;
        _pageActionRepository = pageActionRepository;

        _propertyMappingService = propertyMappingService;
        _companyProfileRepository = companyProfileRepository;
    }

    private async Task<List<AppClaimDto>> GetUserAndRoleClaims(User appUser, CompanyProfile companyProfile)
    {
        var userClaims = await _userClaimRepository.All.Include(c => c.PageAction).Where(c => c.UserId == appUser.Id).ToListAsync();
        var roleClaims = await GetRoleClaims(appUser);
        List<AppClaimDto> lstAppClaimDto = new List<AppClaimDto>();
        foreach (var appClaimDto in userClaims)
        {
            var claimType = appClaimDto.PageAction.Code;
            if (!string.IsNullOrEmpty(claimType))
            {
                lstAppClaimDto.Add(new AppClaimDto
                {
                    ClaimType = claimType.ToLower(),
                    ClaimValue = "true"
                });
            }
        }

        foreach (var appClaimDto in roleClaims)
        {
            var claimType = appClaimDto.PageAction.Code;
            if (!string.IsNullOrEmpty(claimType) && !lstAppClaimDto.Any(c => c.ClaimType.ToLower() == appClaimDto.PageAction.Code.ToLower()))
            {
                lstAppClaimDto.Add(new AppClaimDto
                {
                    ClaimType = claimType.ToLower(),
                    ClaimValue = "true"
                });
            }
        }
        //foreach (var screenOperation in screenOperations)
        //{
        //    var claimName = $"{screenOperation.Screen.Name.Replace(" ", "_").ToLower()}_{screenOperation.Operation.Name.Replace(" ", "_").ToLower()}";

        //    if (!lstAppClaimDto.Any(c => c.ClaimType == claimName) && userClaims.Any(c => c.ScreenId == screenOperation.ScreenId && c.OperationId == screenOperation.OperationId))
        //    {
        //        lstAppClaimDto.Add(new AppClaimDto
        //        {
        //            ClaimType = claimName,
        //            ClaimValue = "true"
        //        });
        //    }
        //    if (!lstAppClaimDto.Any(c => c.ClaimType == claimName) && roleClaims.Any(c => c.ScreenId == screenOperation.ScreenId && c.OperationId == screenOperation.OperationId))
        //    {
        //        lstAppClaimDto.Add(new AppClaimDto
        //        {
        //            ClaimType = claimName,
        //            ClaimValue = "true"
        //        });
        //    }
        //}

        lstAppClaimDto.Add(new AppClaimDto { ClaimType = "licensekey", ClaimValue = HttpUtility.UrlEncode(companyProfile.LicenseKey) ?? "" });
        lstAppClaimDto.Add(new AppClaimDto { ClaimType = "purchasecode", ClaimValue = HttpUtility.UrlEncode(companyProfile.PurchaseCode) ?? "" });

        return lstAppClaimDto;
    }

    public async Task<List<User>> GetUsersByIds(List<Guid> ids)
    {
        return await All.Where(cs => ids.Contains(cs.Id)).Distinct().ToListAsync();
    }
    public async Task<List<Guid>> GetUsersByRoleId(Guid roleId)
    {
        return await All.Where(cs => cs.UserRoles.Any(c => c.RoleId == roleId)).Select(c => c.Id).ToListAsync();
    }


    private async Task<List<RoleClaim>> GetRoleClaims(User appUser)
    {
        List<RoleClaim> lstRoleClaim = new List<RoleClaim>();
        if (appUser.IsSuperAdmin)
        {
            var roleClaims = await _roleClaimRepository.All.Include(c => c.PageAction).ToListAsync();
            foreach (var roleClaim in roleClaims)
            {
                lstRoleClaim.Add(roleClaim);
            }
            return roleClaims;
        }
        else
        {
            var rolesIds = await _userRoleRepository.AllIncluding(u => u.Role)
            .Where(c => !c.Role.IsDeleted && c.UserId == appUser.Id)
            .Select(c => c.RoleId)
            .ToListAsync();

            var roleClaims = await _roleClaimRepository.All.Include(c => c.PageAction).Where(c => rolesIds.Contains(c.RoleId)).ToListAsync();

            //List<RoleClaim> lstRoleClaim = new List<RoleClaim>();
            foreach (var roleClaim in roleClaims)
            {
                if (!lstRoleClaim.Any(c => c.PageActionId == roleClaim.PageActionId))
                {
                    lstRoleClaim.Add(roleClaim);
                }
            }
            return lstRoleClaim;
        }

    }

    public async Task<UserAuthDto> BuildUserAuthObject(User appUser)
    {
        var companyProfile = _companyProfileRepository.All.FirstOrDefault();
        UserAuthDto ret = new UserAuthDto();
        List<AppClaimDto> appClaims = new List<AppClaimDto>();
        // Set User Properties
        ret.Id = appUser.Id.ToString();
        ret.UserName = appUser.UserName;
        ret.FirstName = appUser.FirstName;
        ret.LastName = appUser.LastName;
        ret.Email = appUser.Email;
        ret.PhoneNumber = appUser.PhoneNumber;
        ret.IsSuperAdmin = appUser.IsSuperAdmin;
        ret.IsAuthenticated = true;
        // Get all claims for this user
        var appClaimDtos = await GetUserAndRoleClaims(appUser, companyProfile);
        ret.Claims = appClaimDtos;
        var claims = appClaimDtos.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        // Set JWT bearer token
        ret.BearerToken = BuildJwtToken(ret, claims, appUser.Id, companyProfile);
        return ret;
    }
    protected string BuildJwtToken(UserAuthDto authUser, IList<Claim> claims, Guid Id, CompanyProfile companyProfile)
    {

        SymmetricSecurityKey key = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_settings.Key));
        claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub.ToString(), Id.ToString()));
        claims.Add(new Claim("Email", authUser.Email));
        //claims.Add(new Claim("LicenseKey", HttpUtility.UrlEncode(companyProfile.LicenseKey) ?? ""));
        //claims.Add(new Claim("PurchaseCode", HttpUtility.UrlEncode(companyProfile.PurchaseCode) ?? ""));
        // Create the JwtSecurityToken object
        var token = new JwtSecurityToken(
          issuer: _settings.Issuer,
          audience: _settings.Audience,
          claims: claims,
          notBefore: DateTime.UtcNow,
          expires: DateTime.UtcNow.AddMinutes(
              _settings.MinutesToExpiration),
          signingCredentials: new SigningCredentials(key,
                      SecurityAlgorithms.HmacSha256)
        );
        // Create a string representation of the Jwt token
        return new JwtSecurityTokenHandler().WriteToken(token); ;
    }

    public async Task<UserList> GetUsers(UserResource userResource)
    {
        var collectionBeforePaging = All;
        collectionBeforePaging =
           collectionBeforePaging.ApplySort(userResource.OrderBy,
           _propertyMappingService.GetPropertyMapping<UserDto, User>());

        if (!string.IsNullOrWhiteSpace(userResource.SearchQuery))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => !c.IsSystemUser && EF.Functions.Like(c.FirstName, $"%{userResource.SearchQuery}%")
                || EF.Functions.Like(c.LastName, $"%{userResource.SearchQuery}%")
                || EF.Functions.Like(c.Email, $"%{userResource.SearchQuery}%"));
        }

        var users = new UserList();

        return await users.Create(
            collectionBeforePaging,
            userResource.Skip,
            userResource.PageSize
            );
    }
}
