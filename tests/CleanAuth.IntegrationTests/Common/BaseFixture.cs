using CleanAuth.Infrastructure.Common.Persistence;
using CleanAuth.Infrastructure.Security.PasswordHasher;
using CleanAuth.Infrastructure.Security.TokenGenerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanAuth.IntegrationTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; } = new("pt_BR");

    public AppDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }

    public BCryptPasswordHasher GetBCryptPasswordHasher()
    {
        var optionsWrapper = new OptionsWrapper<BCryptPasswordHasherSettings>(
            new BCryptPasswordHasherSettings { WorkFactor = 12 }
        );

        return new BCryptPasswordHasher(optionsWrapper);
    }

    public JwtTokenGenerator GetJwtTokenGenerator()
    {
        var optionsWrapper = new OptionsWrapper<JwtSettings>(
            new JwtSettings()
            {
                Audience = "test",
                Issuer = "test",
                TokenExpirationInMinutes = 60,
                Secret = "_integration_test_password_jwt_token_generator_mock_data",
            }
        );

        return new JwtTokenGenerator(optionsWrapper);
    }
}
