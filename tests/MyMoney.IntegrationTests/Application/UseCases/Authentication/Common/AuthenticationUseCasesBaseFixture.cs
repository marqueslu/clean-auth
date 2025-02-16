using MyMoney.Domain.Entities;
using MyMoney.IntegrationTests.Common;

namespace MyMoney.IntegrationTests.Application.UseCases.Authentication.Common;

public class AuthenticationUseCasesBaseFixture : BaseFixture
{
    public User GetValidUser(string? password = null) =>
        new(
            GetValidUserName(),
            GetValidUserEmail(),
            password is not null ? password : GetValidUserPassword(),
            GetBCryptPasswordHasher()
        );

    public string GetValidUserName()
    {
        var userName = Faker.Person.FullName;
        if (userName.Length > 100)
        {
            userName = userName[..100];
        }

        return userName;
    }

    public string GetValidUserEmail()
    {
        return Faker.Person.Email;
    }

    public string GetValidUserPassword()
    {
        return Faker.Internet.Password(8);
    }
}
