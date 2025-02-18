using CleanAuth.Application.UseCases.Authentication.Commands.SignUp;
using CleanAuth.UnitTests.Application.UseCases.Authentication.Common;

namespace CleanAuth.UnitTests.Application.UseCases.Authentication.Commands.SignUp;

[CollectionDefinition(nameof(SignUpCommandHandlerTestFixture))]
public class SignUpCommandHandlerTestFixtureCollection
    : ICollectionFixture<SignUpCommandHandlerTestFixture> { }

public class SignUpCommandHandlerTestFixture : AuthenticationUseCasesBaseFixture
{
    public SignUpCommand GetExampleInput() =>
        new(GetValidUserName(), GetValidUserEmail(), GetValidUserPassword());
}
