using MyMoney.Domain.Entities;
using MyMoney.e2eTests.Base;
using MyMoney.e2eTests.Contracts;

namespace MyMoney.e2eTests.Api.Authentication.Common;

public class AuthenticationBaseFixture : BaseFixture<IAuthenticationApiClient>
{
    public readonly AuthenticationPersistence Persistence;

    protected AuthenticationBaseFixture()
    {
        Persistence = new AuthenticationPersistence(CreateDbContext());
    }

    protected string GetValidUserName()
    {
        var userName = Faker.Person.FullName;
        if (userName.Length > 100)
        {
            userName = userName[..100];
        }

        return userName;
    }

    protected string GetValidUserEmail()
    {
        return Faker.Person.Email;
    }

    protected string GetValidUserPassword()
    {
        return Faker.Internet.Password(8);
    }

    public User GetValidUserFromCommand(string name, string email, string password)
        => new(name, email, password, GetBCryptPasswordHasher());
}