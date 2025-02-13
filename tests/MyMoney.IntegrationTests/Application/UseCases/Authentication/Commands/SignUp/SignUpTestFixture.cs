using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.IntegrationTests.Application.UseCases.Authentication.Common;

namespace MyMoney.IntegrationTests.Application.UseCases.Authentication.Commands.SignUp;

[CollectionDefinition(nameof(SignUpTestFixture))]
public class RegisterUserTestFixtureCollection : ICollectionFixture<SignUpTestFixture>
{
}

public class SignUpTestFixture : AuthenticationUseCasesBaseFixture
{
    public SignUpCommand GetCommand()
        => new(GetValidUserName(), GetValidUserEmail(), GetValidUserPassword());

    public SignUpCommand GetCommandWithTooLongName()
        => new(Faker.Lorem.Sentence(range: 101), GetValidUserEmail(), GetValidUserPassword());
    
    public SignUpCommand GetCommandWithTooShortName()
        => new("abc", GetValidUserEmail(), GetValidUserPassword());
    
    public SignUpCommand GetCommandWithTooShortPassword()
        => new(GetValidUserName(), GetValidUserEmail(), Faker.Internet.Password(length: 7));
    
    public SignUpCommand GetCommandWithInvalidEmail()
        => new(GetValidUserName(), "test.com", GetValidUserPassword());
    
    public SignUpCommand GetCommandWithNullName()
        => new("", GetValidUserEmail(), GetValidUserPassword());
}