using DomainEntity = CleanAuth.Domain.Entities;

namespace CleanAuth.IntegrationTests.Common.User;

public class UserBaseFixture : BaseFixture
{
    public DomainEntity.User GetValidUser(string? password = null) =>
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
