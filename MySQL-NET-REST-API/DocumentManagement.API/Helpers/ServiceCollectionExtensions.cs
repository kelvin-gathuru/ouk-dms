using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.API.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLicenseChecker(this IServiceCollection services)
    {
        //services.AddHostedService<LicenseCheckerService>();
        return services;
    }
}