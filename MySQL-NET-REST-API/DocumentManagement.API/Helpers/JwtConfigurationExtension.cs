using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Api.Helpers;

public static class JwtAuthenticationConfigurationExtension
{
    public static void AddJwtAutheticationConfiguration(
        this IServiceCollection services,
        JwtSettings settings)
    {
        // Register Jwt as the Authentication service
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", jwtBearerOptions =>
        {
            jwtBearerOptions.TokenValidationParameters =
          new TokenValidationParameters
          {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(settings.Key)),
              ValidateIssuer = true,
              ValidIssuer = settings.Issuer,

              ValidateAudience = true,
              ValidAudience = settings.Audience,

              ValidateLifetime = true,
              ClockSkew = TimeSpan.FromMinutes(
                     settings.MinutesToExpiration)
          };
            jwtBearerOptions.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    // var accessToken = context.SecurityToken as JsonWebToken;
                    if (context.SecurityToken is JsonWebToken accessToken)
                    {
                        var userName = accessToken.Claims.FirstOrDefault(a => a.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub)?.Value;
                        var email = accessToken.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
                        context.HttpContext.Items["Id"] = userName;
                        var userInfoToken = context.HttpContext.RequestServices.GetRequiredService<UserInfoToken>();
                        userInfoToken.Id = Guid.Parse(userName);
                        userInfoToken.Email = email;
                        userInfoToken.IsSuperAdmin = accessToken.Claims.FirstOrDefault(a => a.Type == "IsSuperAdmin")?.Value.ToLower() == "true";
                        userInfoToken.IsClient = accessToken.Claims.FirstOrDefault(a => a.Type == "IsClient")?.Value.ToLower() == "true";
                    }
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization();
    }
}
