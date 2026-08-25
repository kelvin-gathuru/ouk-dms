using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DocumentManagement.Api.Helpers;

/// <summary>
/// Authenticates requests to the Intranet API using an X-Api-Key header.
/// The accepted keys are configured in appsettings under "Intranet:ApiKeys"
/// (comma-separated). Used by the intranet application to read documents
/// that were explicitly marked as intranet accessible.
/// </summary>
public class IntranetApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "IntranetApiKey";
    public const string ApiKeyHeaderName = "X-Api-Key";

    private readonly IConfiguration _configuration;

    public IntranetApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var configuredKeys = _configuration["Intranet:ApiKeys"] ?? string.Empty;
        var keys = configuredKeys
            .Split(',', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)
            .ToList();

        if (!keys.Contains(providedApiKey.ToString()))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "intranet"),
            new Claim("IsIntranet", "true")
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
