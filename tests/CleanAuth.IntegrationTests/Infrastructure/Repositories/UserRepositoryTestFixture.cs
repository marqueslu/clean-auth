using CleanAuth.Domain.Entities;
using CleanAuth.IntegrationTests.Common;

namespace CleanAuth.IntegrationTests.Infrastructure.Repositories;

[CollectionDefinition(nameof(UserRepositoryTestFixture))]
public class UserRepositoryTestFixtureCollection : ICollectionFixture<UserRepositoryTestFixture> { }

public class UserRepositoryTestFixture : BaseFixture
{
    public User GetValidUser() =>
        new(
            GetValidUserName(),
            GetValidUserEmail(),
            GetValidUserPassword(),
            GetBCryptPasswordHasher()
        );

    private string GetValidUserName()
    {
        var userName = Faker.Person.FullName;
        if (userName.Length > 100)
        {
            userName = userName[..100];
        }

        return userName;
    }

    private string GetValidUserEmail()
    {
        return Faker.Person.Email;
    }

    private string GetValidUserPassword()
    {
        return Faker.Internet.Password(8);
    }
}
