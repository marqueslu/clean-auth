using System.Net;

namespace CleanAuth.e2eTests.Api.User.GetProfile;

[Collection(nameof(GetProfileApiTestFixture))]
public class GetProfileApiTest(GetProfileApiTestFixture fixture) : IDisposable
{
    [Fact]
    public async Task Given_GetProfile_When_UserExists_Then_ShouldReturns()
    {
        var password = fixture.GetValidUserPassword();
        var exampleUser = fixture.GetUser(password);
        await fixture.Persistence.Save(exampleUser);

        var signInQuery = fixture.GetSignInQuery(exampleUser.Email, password);

        var signInResponse = await fixture.ApiClient.SignInAsync(signInQuery);
        var getProfileQuery = fixture.GetProfileQuery(exampleUser.Id);

        var response = await fixture.ApiClient.GetProfileAsync(
            $"Bearer {signInResponse.Content!.Token}",
            getProfileQuery.Id
        );

        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content!.Name.ShouldBe(exampleUser.Name);
        response.Content.Email.ShouldBe(exampleUser.Email);
    }

    [Fact]
    public async Task Given_GetProfile_When_InvalidTokenIsPassed_Then_ShouldReturnStatusCode403()
    {
        var password = fixture.GetValidUserPassword();
        var exampleUser = fixture.GetUser(password);
        await fixture.Persistence.Save(exampleUser);

        var sutUser = fixture.GetUser(password);
        await fixture.Persistence.Save(sutUser);

        var signInQuery = fixture.GetSignInQuery(exampleUser.Email, password);
        var signInResponse = await fixture.ApiClient.SignInAsync(signInQuery);

        var query = fixture.GetProfileQuery(sutUser.Id);

        var response = await fixture.ApiClient.GetProfileAsync(
            $"Bearer {signInResponse.Content!.Token}",
            query.Id
        );
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    public void Dispose() => fixture.CleanPersistence();
}
