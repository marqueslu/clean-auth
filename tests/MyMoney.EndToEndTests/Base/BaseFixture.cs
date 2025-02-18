using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MyMoney.Infrastructure.Common.Persistence;
using MyMoney.Infrastructure.Security.PasswordHasher;

using Refit;

namespace MyMoney.e2eTests.Base;

public class BaseFixture<T> : IDisposable
{
    public T ApiClient { get; set; }
    public Faker Faker { get; private set; }
    private CustomWebApplicationFactory WebApplicationFactory { get; set; }
    private HttpClient HttpClient { get; set; }
    private string _dbConnectionString;

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        ConfigureApiClient();
        ConfigureConnectionString();
    }

    public void CleanPersistence()
    {
        var context = CreateDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        WebApplicationFactory.Dispose();
    }

    protected BCryptPasswordHasher GetBCryptPasswordHasher()
    {
        var optionsWrapper =
            new OptionsWrapper<BCryptPasswordHasherSettings>(new BCryptPasswordHasherSettings { WorkFactor = 12 });

        return new BCryptPasswordHasher(optionsWrapper);
    }

    protected AppDbContext CreateDbContext()
    {
        var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_dbConnectionString).Options);
        return context;
    }

    private void ConfigureConnectionString()
    {
        var configuration = WebApplicationFactory.Services.GetRequiredService<IConfiguration>();
        _dbConnectionString = configuration.GetConnectionString("MyMoneyDb");
    }

    private void ConfigureApiClient()
    {
        WebApplicationFactory = new CustomWebApplicationFactory();
        HttpClient = WebApplicationFactory.CreateClient();
        ApiClient = RestService.For<T>(HttpClient);
    }
}