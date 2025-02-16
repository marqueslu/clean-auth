using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.Application.UseCases.Authentication.Queries.SignIn;
using MyMoney.Domain.Entities;
using MyMoney.e2eTests.Api.Authentication.Common;

namespace MyMoney.e2eTests.Api.Authentication.SignIn;

[CollectionDefinition(nameof(SignInApiTestFixture))]
public class SignInApiTestFixtureCollection : ICollectionFixture<SignInApiTestFixture> { }

public class SignInApiTestFixture : AuthenticationBaseFixture
{
    public SignInQuery GetQuery() => new(GetValidUserEmail(), GetValidUserPassword());

    public SignInQuery GetQuery(string email, string password) => new(email, password);

    public User GetUser(string password) =>
        new(GetValidUserName(), GetValidUserEmail(), password, GetBCryptPasswordHasher());

    public SignInQuery GetInvalidQuery() => new("test", GetValidUserPassword());
}
