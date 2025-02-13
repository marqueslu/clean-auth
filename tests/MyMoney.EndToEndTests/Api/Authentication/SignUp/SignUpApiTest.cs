using System.Net;

namespace MyMoney.e2eTests.Api.Authentication.SignUp;

[Collection(nameof(SignUpApiTestFixture))]
public class SignUpApiTest(SignUpApiTestFixture fixture) : IDisposable
{
    [Fact]
    public async Task Given_SignUp_When_AllDataIsPassedCorrectly_Then_ShouldRegister()
    {
        var command = fixture.GetCommand();

        var response = await fixture.ApiClient.SignUpAsync(command);
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content!.Name.ShouldBe(command.Name);
        response.Content.Email.ShouldBe(command.Email);
        response.Content.Token.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task Given_SignUp_When_AllIncorrectDataIsPassed_Then_ShouldReturnStatusCode422()
    {
        var command = fixture.GetInvalidCommand();
        
        var response = await fixture.ApiClient.SignUpAsync(command);
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Given_SignUp_When_AlreadyExistsUserWithSameEmail_Then_ShouldReturnStatusCode409()
    {
        var command = fixture.GetCommand();
        var user = fixture.GetValidUserFromCommand(command.Name, command.Email, command.Password);
        await fixture.Persistence.Save(user);
        
        var response = await fixture.ApiClient.SignUpAsync(command);
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    public void Dispose()
        =>
            fixture.CleanPersistence();
}