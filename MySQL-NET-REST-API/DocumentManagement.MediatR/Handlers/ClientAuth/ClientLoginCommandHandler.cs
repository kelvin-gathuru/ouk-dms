using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ClientLoginCommandHandler : IRequestHandler<ClientLoginCommand, ServiceResponse<LoginResponseDto>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly JwtSettings _jwtSettings;

        public ClientLoginCommandHandler(
            IClientRepository clientRepository,
            JwtSettings jwtSettings)
        {
            _clientRepository = clientRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Handle(ClientLoginCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.FindBy(c => c.Email == request.Email).FirstOrDefaultAsync();
            if (client == null)
            {
                return ServiceResponse<LoginResponseDto>.ReturnFailed(401, "Invalid email or password.");
            }

            if (!client.IsActivated)
            {
                return ServiceResponse<LoginResponseDto>.ReturnFailed(403, "Account is not activated.");
            }

            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, client.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ServiceResponse<LoginResponseDto>.ReturnFailed(401, "Invalid email or password.");
            }

            var token = BuildJwtToken(client);
            
            var response = new LoginResponseDto
            {
                Token = token,
                Id = client.Id,
                Email = client.Email,
                CompanyName = client.CompanyName,
                ContactPerson = client.ContactPerson
            };

            return ServiceResponse<LoginResponseDto>.ReturnResultWith200(response, "Login successful.");
        }

        private string BuildJwtToken(Client client)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()),
                new Claim("Email", client.Email),
                new Claim("IsClient", "true"),
                new Claim("CompanyName", client.CompanyName)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.MinutesToExpiration),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
