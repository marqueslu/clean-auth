using Microsoft.OpenApi.Models;
using CleanAuth.Api.Configurations;

namespace CleanAuth.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAndConfigureControllers();

        return services;
    }
}
