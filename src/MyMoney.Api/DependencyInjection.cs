using Microsoft.OpenApi.Models;
using MyMoney.Api.Configurations;

namespace MyMoney.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAndConfigureControllers();

        return services;
    }
}
