using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using MyMoney.Infrastructure.Common.Persistence;

namespace MyMoney.e2eTests.Base;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        const string environment = "e2e";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        builder.UseEnvironment(environment);
        builder.ConfigureTestServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider
                .GetService<AppDbContext>();
            ArgumentNullException.ThrowIfNull(context);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });

        base.ConfigureWebHost(builder);
    }
}