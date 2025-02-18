using CleanAuth.Application.UseCases.User.Queries.Profile;
using CleanAuth.IntegrationTests.Application.UseCases.Authentication.Common;

namespace CleanAuth.IntegrationTests.Application.UseCases.User.Queries.Profile;

[CollectionDefinition(nameof(GetProfileTestFixture))]
public class GetProfileTestFixtureCollection : ICollectionFixture<GetProfileTestFixture> { }

public class GetProfileTestFixture : AuthenticationUseCasesBaseFixture
{
    public GetProfileQuery GetQuery(Guid id) => new(id);
}
