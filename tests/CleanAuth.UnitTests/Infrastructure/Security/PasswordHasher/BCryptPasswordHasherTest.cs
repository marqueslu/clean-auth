using CleanAuth.Infrastructure.Security.PasswordHasher;
using Microsoft.Extensions.Options;

namespace CleanAuth.UnitTests.Infrastructure.Security.PasswordHasher;

public class BCryptPasswordHasherTest
{
    private readonly Mock<IOptions<BCryptPasswordHasherSettings>> _settingsMock;

    public BCryptPasswordHasherTest()
    {
        _settingsMock = new Mock<IOptions<BCryptPasswordHasherSettings>>();
        _settingsMock
            .Setup(x => x.Value)
            .Returns(new BCryptPasswordHasherSettings { WorkFactor = 12 });
    }

    [Fact]
    public void Given_HashPassword_When_HashedPassword_Then_ShouldReturnHashedPassword()
    {
        string password = "mySecurePassword";
        string hashedPassword = GetBCryptPasswordHasher().HashPassword(password);

        hashedPassword.ShouldNotBeNullOrEmpty();
        hashedPassword.ShouldNotBe(password);
    }

    [Fact]
    public void Given_HashPassword_When_VerifyPassword_Then_ShouldReturnTrueForValidPassword()
    {
        string password = "mySecurePassword";
        string hashedPassword = GetBCryptPasswordHasher().HashPassword(password);
        bool result = GetBCryptPasswordHasher().VerifyPassword(password, hashedPassword);

        result.ShouldBeTrue();
    }

    [Fact]
    public void Given_HashPassword_When_VerifyPassword_Then_ShouldReturnFalseForInvalidPassword()
    {
        string password = "mySecurePassword";
        string hashedPassword = GetBCryptPasswordHasher().HashPassword(password);
        password = "myInsecurePassword";
        bool result = GetBCryptPasswordHasher().VerifyPassword(password, hashedPassword);

        result.ShouldBeFalse();
    }

    private BCryptPasswordHasher GetBCryptPasswordHasher() => new(_settingsMock.Object);
}
