using MyMoney.UnitTests.Common.User;

namespace MyMoney.UnitTests.Domain.Entities.User;

[CollectionDefinition(nameof(UserTestFixture))]
public class UserTestFixtureCollection : ICollectionFixture<UserTestFixture>
{
}

public class UserTestFixture : UserBaseFixture
{
}