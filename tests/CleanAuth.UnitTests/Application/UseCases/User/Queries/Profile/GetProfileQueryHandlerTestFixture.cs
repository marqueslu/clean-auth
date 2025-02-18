using CleanAuth.Application.UseCases.User.Queries.Profile;
using CleanAuth.UnitTests.Common.User;

namespace CleanAuth.UnitTests.Application.UseCases.User.Queries.Profile;

[CollectionDefinition(nameof(GetProfileQueryHandlerTestFixture))]
public class GetProfileQueryHandlerTestFixtureCollection
    : ICollectionFixture<GetProfileQueryHandlerTestFixture> { }

public class GetProfileQueryHandlerTestFixture : UserBaseFixture
{
    public GetProfileQuery GetExampleInput(Guid id) => new(id);

    public CleanAuth.Domain.Entities.User GetUser() =>
        new(
            GetValidUserName(),
            GetValidUserEmail(),
            GetValidUserPassword(),
            GetPasswordHasherMock().Object
        );
}
