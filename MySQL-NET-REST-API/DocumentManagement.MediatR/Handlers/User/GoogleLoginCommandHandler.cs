using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DocumentManagement.MediatR.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, UserAuthDto>
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public GoogleLoginCommandHandler(
            IUserRepository userRepository,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserAuthDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var clientId = _configuration["GoogleAuth:ClientId"];
            if (string.IsNullOrEmpty(request.IdToken) || string.IsNullOrEmpty(clientId))
            {
                return new UserAuthDto
                {
                    StatusCode = 400,
                    Messages = new List<string> { "Invalid request." }
                };
            }

            ClaimsPrincipal principal;
            try
            {
                var jwksJson = await _httpClient.GetStringAsync("https://www.googleapis.com/oauth2/v3/certs");
                var jwks = new JsonWebKeySet(jwksJson);
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuers = new[]
                    {
                        "https://accounts.google.com",
                        "accounts.google.com",
                        "http://accounts.google.com"
                    },
                    ValidAudience = clientId,
                    IssuerSigningKeys = jwks.Keys,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    RequireExpirationTime = true
                };
                var handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(request.IdToken, validationParameters, out _);
            }
            catch (Exception)
            {
                return new UserAuthDto
                {
                    StatusCode = 401,
                    Messages = new List<string> { "Google token validation failed." }
                };
            }

            var email = principal.FindFirst(ClaimTypes.Email)?.Value
                        ?? principal.FindFirst("email")?.Value;
            var givenName = principal.FindFirst(ClaimTypes.GivenName)?.Value
                            ?? principal.FindFirst("given_name")?.Value;
            var familyName = principal.FindFirst(ClaimTypes.Surname)?.Value
                             ?? principal.FindFirst("family_name")?.Value;

            if (string.IsNullOrEmpty(email) || !email.EndsWith("@ouk.ac.ke", StringComparison.OrdinalIgnoreCase))
            {
                return new UserAuthDto
                {
                    StatusCode = 403,
                    Messages = new List<string> { "Only @ouk.ac.ke accounts are allowed." }
                };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = givenName,
                    LastName = familyName,
                    EmailConfirmed = true,
                    IsSuperAdmin = false,
                    IsSystemUser = false,
                    IsDeleted = false
                };
                var password = "G00gle!" + Guid.NewGuid().ToString("N");
                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    return new UserAuthDto
                    {
                        StatusCode = 500,
                        Messages = new List<string> { "Unable to create user." }
                    };
                }
            }

            var ssoRole = _configuration["GoogleAuth:DefaultRole"] ?? "User";
            if (!await _userManager.IsInRoleAsync(user, ssoRole))
            {
                await _userManager.AddToRoleAsync(user, ssoRole);
            }

            return await _userRepository.BuildUserAuthObject(user);
        }
    }
}
