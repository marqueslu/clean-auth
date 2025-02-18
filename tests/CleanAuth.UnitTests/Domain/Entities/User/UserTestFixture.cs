using CleanAuth.UnitTests.Common.User;

namespace CleanAuth.UnitTests.Domain.Entities.User;

[CollectionDefinition(nameof(UserTestFixture))]
public class UserTestFixtureCollection : ICollectionFixture<UserTestFixture> { }

public class UserTestFixture : UserBaseFixture { }
