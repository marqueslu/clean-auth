using Microsoft.Extensions.Options;
using MyMoney.Domain.Interfaces.Security;

namespace MyMoney.Infrastructure.Security.PasswordHasher;

public class BCryptPasswordHasher : IPasswordHasher
{
    private readonly BCryptPasswordHasherSettings _settings;

    public BCryptPasswordHasher(IOptions<BCryptPasswordHasherSettings> settings)
    {
        _settings = settings.Value;
    }

    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, _settings.WorkFactor);

    public bool VerifyPassword(string providedPassword, string hashedPassword) =>
        BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
}
