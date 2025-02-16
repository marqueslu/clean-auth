using MyMoney.Application.UseCases.Authentication.Queries.SignIn;
using MyMoney.IntegrationTests.Application.UseCases.Authentication.Common;

namespace MyMoney.IntegrationTests.Application.UseCases.Authentication.Queries.SignIn;

[CollectionDefinition(nameof(SignInTestFixture))]
public class SignInTestFixtureCollection : ICollectionFixture<SignInTestFixture> { }

public class SignInTestFixture : AuthenticationUseCasesBaseFixture
{
    public SignInQuery GetQuery(string email, string password) => new(email, password);
}
