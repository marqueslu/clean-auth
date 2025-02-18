using MyMoney.Application.Exceptions;
using MyMoney.Application.Intefaces;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Application.UseCases.Common;
using MyMoney.Domain.Interfaces.Security;
using MyMoney.Domain.Repository;

namespace MyMoney.Application.UseCases.Authentication.Queries.SignIn;

public class SignInQueryHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
) : IQueryHandler<SignInQuery, AuthResult>
{
    public async Task<AuthResult> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(request.Email, cancellationToken);
        NotFoundException.ThrowIfNull(user, "User not found.");
        var validPassword = passwordHasher.VerifyPassword(request.Password, user!.Password);
        DivergentDataException.ThrowIfNoValid(validPassword, "Wrong password.");
        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Email);
        return AuthResult.GenerateResult(user, token);
    }
}
