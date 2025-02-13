using MyMoney.Domain.Interfaces.Security;
using MyMoney.UnitTests.Common;

namespace MyMoney.UnitTests.Domain.Entities.User;

[CollectionDefinition(nameof(UserTestFixture))]
public class UserTestFixtureCollection : ICollectionFixture<UserTestFixture>
{
}

public class UserTestFixture : BaseFixture
{
    public string GetValidUserName()
    {
        var userName = Faker.Person.FullName;
        if (userName.Length > 100)
        {
            userName = userName[..100];
        }

        return userName;
    }
    
    public string GetValidUserPassword()
    {
        return Faker.Internet.Password(8);
    }

    public Mock<IPasswordHasher> GetPasswordHasherMock() => new();

    public (string, string, string) GetValidUserData() =>
        (GetValidUserName(), GetValidUserEmail(), GetValidUserPassword());
    
    private string GetValidUserEmail()
    {
        return Faker.Person.Email;
    }
}