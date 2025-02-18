using CleanAuth.Application.UseCases.Authentication.Queries.SignIn;
using CleanAuth.Application.UseCases.User.Queries.Profile;
using CleanAuth.e2eTests.Api.User.Common;

namespace CleanAuth.e2eTests.Api.User.GetProfile;

[CollectionDefinition(nameof(GetProfileApiTestFixture))]
public class GetProfileApiTestFixtureCollection : ICollectionFixture<GetProfileApiTestFixture> { }

public class GetProfileApiTestFixture : UserBaseFixture
{
    public SignInQuery GetSignInQuery() => new(GetValidUserEmail(), GetValidUserPassword());

    public SignInQuery GetSignInQuery(string email, string password) => new(email, password);

    public GetProfileQuery GetProfileQuery(Guid id) => new(id);

    public Domain.Entities.User GetUser(string password) =>
        new(GetValidUserName(), GetValidUserEmail(), password, GetBCryptPasswordHasher());
}
