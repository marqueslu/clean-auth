using CleanAuth.Domain.Entities;
using CleanAuth.IntegrationTests.Common;

namespace CleanAuth.IntegrationTests.Infrastructure.Persistence.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection : ICollectionFixture<UnitOfWorkTestFixture> { }

public class UnitOfWorkTestFixture : BaseFixture
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
