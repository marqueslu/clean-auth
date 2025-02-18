using MyMoney.Application.UseCases.User.Queries.Profile;
using MyMoney.UnitTests.Common.User;

namespace MyMoney.UnitTests.Application.UseCases.User.Queries.Profile;

[CollectionDefinition(nameof(GetProfileQueryHandlerTestFixture))]
public class GetProfileQueryHandlerTestFixtureCollection
    : ICollectionFixture<GetProfileQueryHandlerTestFixture>
{
}

public class GetProfileQueryHandlerTestFixture : UserBaseFixture
{
    public GetProfileQuery GetExampleInput(Guid id) => new(id);

    public MyMoney.Domain.Entities.User GetUser() =>
        new(
            GetValidUserName(),
            GetValidUserEmail(),
            GetValidUserPassword(),
            GetPasswordHasherMock().Object
        );
}