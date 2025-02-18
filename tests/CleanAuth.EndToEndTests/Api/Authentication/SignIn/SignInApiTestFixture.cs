using CleanAuth.Application.UseCases.Authentication.Queries.SignIn;
using CleanAuth.e2eTests.Api.Authentication.Common;
using DomainEntity = CleanAuth.Domain.Entities;

namespace CleanAuth.e2eTests.Api.Authentication.SignIn;

[CollectionDefinition(nameof(SignInApiTestFixture))]
public class SignInApiTestFixtureCollection : ICollectionFixture<SignInApiTestFixture> { }

public class SignInApiTestFixture : AuthenticationBaseFixture
{
    public SignInQuery GetQuery() => new(GetValidUserEmail(), GetValidUserPassword());

    public SignInQuery GetQuery(string email, string password) => new(email, password);

    public DomainEntity.User GetUser(string password) =>
        new(GetValidUserName(), GetValidUserEmail(), password, GetBCryptPasswordHasher());

    public SignInQuery GetInvalidQuery() => new("test", GetValidUserPassword());
}
