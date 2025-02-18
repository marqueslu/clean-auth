using CleanAuth.Application.Intefaces;
using CleanAuth.UnitTests.Common.User;
using DomainEntity = CleanAuth.Domain.Entities;

namespace CleanAuth.UnitTests.Application.UseCases.Authentication.Common;

public class AuthenticationUseCasesBaseFixture : UserBaseFixture
{
    public DomainEntity.User GetExistentUser() => new();

    public Mock<IJwtTokenGenerator> GetJwtTokenGeneratorMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}
