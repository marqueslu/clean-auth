using System.Net;
using System.Threading.Tasks;

namespace MyMoney.e2eTests.Api.Authentication.SignIn;

[Collection(nameof(SignInApiTestFixture))]
public class SignInApiTest(SignInApiTestFixture fixture) : IDisposable
{
    [Fact]
    public async Task Given_SignIn_When_UserExists_Then_ShouldReturns()
    {
        var password = fixture.GetValidUserPassword();
        var exampleUser = fixture.GetUser(password);
        await fixture.Persistence.Save(exampleUser);

        var query = fixture.GetQuery(exampleUser.Email, password);

        var response = await fixture.ApiClient.SignInAsync(query);

        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content!.Name.ShouldBe(exampleUser.Name);
        response.Content.Email.ShouldBe(query.Email);
        response.Content.Token.ShouldNotBeNull();
    }

    [Fact]
    public async Task Given_SignIn_When_AllIncorrectDataIsPassed_Then_ShouldReturnStatusCode422()
    {
        var query = fixture.GetInvalidQuery();

        var response = await fixture.ApiClient.SignInAsync(query);
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Given_SignIn_When_UserNotExists_Then_ShouldReturn404()
    {
        var query = fixture.GetQuery();

        var response = await fixture.ApiClient.SignInAsync(query);
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Given_SignIn_When_UserExistsButWrongPasswordIsInforme_Then_ShouldReturn422()
    {
        var password = fixture.GetValidUserPassword();
        var exampleUser = fixture.GetUser(password);
        await fixture.Persistence.Save(exampleUser);

        var query = fixture.GetQuery(exampleUser.Email, "wrongpasswordtest");

        var response = await fixture.ApiClient.SignInAsync(query);
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    public void Dispose() => fixture.CleanPersistence();
}
