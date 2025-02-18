using CleanAuth.Application.Exceptions;
using CleanAuth.Application.Intefaces;
using CleanAuth.Application.UseCases.Authentication.Common;
using CleanAuth.Application.UseCases.Common;

using DomainEntity = CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces.Security;
using CleanAuth.Domain.Repository;

namespace CleanAuth.Application.UseCases.Authentication.Commands.SignUp;

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
        var user = new DomainEntity.User(request.Name, request.Email, request.Password, passwordHasher);
        await userRepository.CreateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Email);
        return AuthResult.GenerateResult(user, token);
    }
}