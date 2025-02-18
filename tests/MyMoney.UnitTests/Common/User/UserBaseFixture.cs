using MyMoney.Domain.Interfaces.Security;
using MyMoney.Domain.Repository;

namespace MyMoney.UnitTests.Common.User;

public class UserBaseFixture : BaseFixture
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
    
    public string GetValidUserEmail()
    {
        return Faker.Person.Email;
    }
    
    public Mock<IUserRepository> GetUserRepositoryMock() => new();
}