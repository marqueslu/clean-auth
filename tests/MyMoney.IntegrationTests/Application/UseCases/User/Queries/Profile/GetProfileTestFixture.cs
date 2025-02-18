using MyMoney.Application.UseCases.User.Queries.Profile;
using MyMoney.IntegrationTests.Application.UseCases.Authentication.Common;

namespace MyMoney.IntegrationTests.Application.UseCases.User.Queries.Profile;

[CollectionDefinition(nameof(GetProfileTestFixture))]
public class GetProfileTestFixtureCollection : ICollectionFixture<GetProfileTestFixture> { }

public class GetProfileTestFixture : AuthenticationUseCasesBaseFixture
{
    public GetProfileQuery GetQuery(Guid id) => new(id);
}
