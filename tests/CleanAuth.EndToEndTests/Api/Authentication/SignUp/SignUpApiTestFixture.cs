using CleanAuth.Application.UseCases.Authentication.Commands.SignUp;
using CleanAuth.e2eTests.Api.Authentication.Common;

namespace CleanAuth.e2eTests.Api.Authentication.SignUp;

[CollectionDefinition(nameof(SignUpApiTestFixture))]
public class SignUpApiTestFixtureCollection : ICollectionFixture<SignUpApiTestFixture> { }

public class SignUpApiTestFixture : AuthenticationBaseFixture
{
    public SignUpCommand GetCommand() =>
        new(GetValidUserName(), GetValidUserEmail(), GetValidUserPassword());

    public SignUpCommand GetInvalidCommand() =>
        new(GetValidUserName(), "test", GetValidUserPassword());
}
