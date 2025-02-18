using CleanAuth.Application.Intefaces;
using CleanAuth.Domain.Interfaces.Security;
using CleanAuth.Domain.Repository;
using CleanAuth.Infrastructure.Common.Persistence;
using CleanAuth.Infrastructure.Repositories;
using CleanAuth.Infrastructure.Security.PasswordHasher;
using CleanAuth.Infrastructure.Security.TokenGenerator;
using CleanAuth.Infrastructure.Security.TokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAuth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddDatabaseConfiguration(configuration)
            .AddSecurity(configuration)
            .AddJwtAuthentication(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CleanAuthDb"))
        );

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<BCryptPasswordHasherSettings>(
            configuration.GetSection(BCryptPasswordHasherSettings.Section)
        );
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services
            .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }

    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == "e2e")
            return app;
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}
