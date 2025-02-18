using MyMoney.Application.Intefaces;

using DomainEntity = MyMoney.Domain.Entities;

using MyMoney.UnitTests.Common.User;

namespace MyMoney.UnitTests.Application.UseCases.Authentication.Common;

public class AuthenticationUseCasesBaseFixture : UserBaseFixture
{
    public DomainEntity.User GetExistentUser()
        => new();

    public Mock<IJwtTokenGenerator> GetJwtTokenGeneratorMock() => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}