using MyMoney.Application.Intefaces;
using MyMoney.Domain.Entities;
using MyMoney.Domain.Interfaces.Security;
using MyMoney.Domain.Repository;
using MyMoney.UnitTests.Common;

namespace MyMoney.UnitTests.Application.UseCases.Authentication.Common;

public class AuthenticationUseCasesBaseFixture : BaseFixture
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

    public string GetInvalidUserName()
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

    public User GetExistentUser()
        => new();


    public (string, string, string) GetValidUserData() =>
        (GetValidUserName(), GetValidUserEmail(), GetValidUserPassword());

    public Mock<IJwtTokenGenerator> GetJwtTokenGeneratorMock() => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
    public Mock<IPasswordHasher> GetPasswordHasherMock() => new();
    public Mock<IUserRepository> GetUserRepositoryMock() => new();
}