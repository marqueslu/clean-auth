using CleanAuth.Application.UseCases.Authentication.Queries.SignIn;
using CleanAuth.IntegrationTests.Application.UseCases.Authentication.Common;

namespace CleanAuth.IntegrationTests.Application.UseCases.Authentication.Queries.SignIn;

[CollectionDefinition(nameof(SignInTestFixture))]
public class SignInTestFixtureCollection : ICollectionFixture<SignInTestFixture> { }

public class SignInTestFixture : AuthenticationUseCasesBaseFixture
{
    public SignInQuery GetQuery(string email, string password) => new(email, password);
}
