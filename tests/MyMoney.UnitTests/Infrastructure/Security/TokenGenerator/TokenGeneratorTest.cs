using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;

using MyMoney.Infrastructure.Security.TokenGenerator;

namespace MyMoney.UnitTests.Infrastructure.Security.TokenGenerator;

public class TokenGeneratorTest
{
    private readonly Mock<IOptions<JwtSettings>> _options = new();
    private readonly JwtSettings _jwtSettings;

    public TokenGeneratorTest()
    {
        _jwtSettings = new JwtSettings
        {
            Audience = "test",
            Issuer = "test",
            TokenExpirationInMinutes = 60,
            Secret = "unit_test_token_validation_test_runner_secret_key"
        };
        _options.Setup(o => o.Value).Returns(_jwtSettings);
    }

    [Fact]
    public void Given_TokenGenerator_When_GenerateJWtToken_Then_ShouldCreateAValidToken()
    {
        var userId = Guid.NewGuid();
        const string userName = "John Doe";
        const string userEmail = "john@doe.com";

        var tokenGenerator = new JwtTokenGenerator(_options.Object);

        var token = tokenGenerator.GenerateToken(userId, userName, userEmail);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Issuer.ShouldBe(_jwtSettings.Issuer);
        jwtToken.Audiences.First().ShouldBe(_jwtSettings.Audience);

        var claims = jwtToken.Claims.ToList();
        claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Name && c.Value == userName);
        claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == userEmail);
        claims.ShouldContain(c => c.Type == "Id" && c.Value == userId.ToString());
    }
}