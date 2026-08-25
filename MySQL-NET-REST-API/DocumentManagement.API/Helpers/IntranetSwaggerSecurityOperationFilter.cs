using System.Collections.Generic;
using DocumentManagement.Api.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DocumentManagement.Api.Helpers;

/// <summary>
/// Applies the IntranetApiKey security requirement to intranet operations
/// (paths under /api/intranet) in every Swagger document. This overrides the
/// global Bearer requirement for those operations so the intranet developers
/// see the correct authentication scheme in the UI.
/// </summary>
public class IntranetSwaggerSecurityOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var relativePath = context.ApiDescription.RelativePath ?? string.Empty;
        if (!relativePath.Contains("/intranet/", System.StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = IntranetApiKeyAuthenticationHandler.SchemeName
                        }
                    },
                    new string[] { }
                }
            }
        };
    }
}
