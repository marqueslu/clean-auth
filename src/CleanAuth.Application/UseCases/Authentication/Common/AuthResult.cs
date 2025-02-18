using DomainEntity = CleanAuth.Domain.Entities;

namespace CleanAuth.Application.UseCases.Authentication.Common;

public record AuthResult(Guid Id, string Name, string Email, string Token)
{
    public static AuthResult GenerateResult(DomainEntity.User user, string token) =>
        new(user.Id, user.Name, user.Email, token);
}
