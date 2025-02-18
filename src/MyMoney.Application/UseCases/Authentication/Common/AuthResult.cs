using DomainEntity = MyMoney.Domain.Entities;

namespace MyMoney.Application.UseCases.Authentication.Common;

public record AuthResult(Guid Id, string Name, string Email, string Token)
{
    public static AuthResult GenerateResult(DomainEntity.User user, string token) =>
        new(user.Id, user.Name, user.Email, token);
}
