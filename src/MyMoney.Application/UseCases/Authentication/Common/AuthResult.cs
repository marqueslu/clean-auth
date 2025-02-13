using MyMoney.Domain.Entities;

namespace MyMoney.Application.UseCases.Authentication.Common;

public record AuthResult(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    string Token
)
{
    public static AuthResult GenerateResult(User user, string token)
        => new(user.Id, user.Name, user.Email, user.CreatedAt, token);
}