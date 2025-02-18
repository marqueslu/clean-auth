using CleanAuth.Application.UseCases.Authentication.Queries.SignIn;
using CleanAuth.Domain.Interfaces.Security;
using CleanAuth.UnitTests.Application.UseCases.Authentication.Common;
using DomainEntity = CleanAuth.Domain.Entities;

namespace CleanAuth.UnitTests.Application.UseCases.Authentication.Queries.SignIn;

[CollectionDefinition(nameof(SignInQueryHandlerTestFixture))]
public class SignQueryHandlerTestFixtureCollection
    : ICollectionFixture<SignInQueryHandlerTestFixture> { }

public class SignInQueryHandlerTestFixture : AuthenticationUseCasesBaseFixture
{
    public SignInQuery GetExampleInput(string email, string password) => new(email, password);

    public DomainEntity.User GetUser(IPasswordHasher passwordHasher) =>
        new(GetValidUserName(), GetValidUserEmail(), GetValidUserPassword(), passwordHasher);
}
