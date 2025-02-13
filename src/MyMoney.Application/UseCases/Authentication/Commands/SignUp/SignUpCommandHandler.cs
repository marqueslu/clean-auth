using MyMoney.Application.Exceptions;
using MyMoney.Application.Intefaces;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Domain.Entities;
using MyMoney.Domain.Interfaces.Security;
using MyMoney.Domain.Repository;

namespace MyMoney.Application.UseCases.Authentication.Commands.SignUp;

public class SignUpCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
) : ICommandHandler<SignUpCommand, AuthResult>
{
    public async Task<AuthResult> Handle(
        SignUpCommand request,
        CancellationToken cancellationToken
    )
    {
        var userExists = await userRepository.FindByEmailAsync(request.Email, cancellationToken);
        ConflictException.ThrowIfNotNull(userExists, "User already exists.");
        var user = new User(request.Name, request.Email, request.Password, passwordHasher);
        await userRepository.CreateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Email);
        return AuthResult.GenerateResult(user, token);
    }
}