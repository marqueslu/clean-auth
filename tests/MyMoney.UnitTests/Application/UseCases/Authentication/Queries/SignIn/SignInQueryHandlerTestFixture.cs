using MyMoney.Application.UseCases.Authentication.Queries.SignIn;
using MyMoney.Domain.Interfaces.Security;
using MyMoney.UnitTests.Application.UseCases.Authentication.Common;

using DomainEntity = MyMoney.Domain.Entities;

namespace MyMoney.UnitTests.Application.UseCases.Authentication.Queries.SignIn;

[CollectionDefinition(nameof(SignInQueryHandlerTestFixture))]
public class SignQueryHandlerTestFixtureCollection
    : ICollectionFixture<SignInQueryHandlerTestFixture> { }

public class SignInQueryHandlerTestFixture : AuthenticationUseCasesBaseFixture
{
    public SignInQuery GetExampleInput(string email, string password) => new(email, password);

    public DomainEntity.User GetUser(IPasswordHasher passwordHasher) =>
        new(GetValidUserName(), GetValidUserEmail(), GetValidUserPassword(), passwordHasher);
}
