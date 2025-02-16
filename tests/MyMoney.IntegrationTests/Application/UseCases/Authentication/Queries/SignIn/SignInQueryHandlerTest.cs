using MyMoney.Application.Exceptions;
using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Application.UseCases.Authentication.Queries.SignIn;
using MyMoney.Infrastructure.Repositories;

namespace MyMoney.IntegrationTests.Application.UseCases.Authentication.Queries.SignIn;

[Collection(nameof(SignInTestFixture))]
public class SignUpCommandHandlerTest(SignInTestFixture fixture)
{
    [Fact]
    public async Task Given_SignInQuery_When_ExecuteHandlerWithCorrectData_Then_ShouldReturnUser()
    {
        var dbContext = fixture.CreateDbContext();
        var password = fixture.GetValidUserPassword();
        var expectedUser = fixture.GetValidUser(password);
        await dbContext.AddAsync(expectedUser, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var repository = new UserRepository(dbContext);
        var jwtTokenGenerator = fixture.GetJwtTokenGenerator();
        var passwordHasher = fixture.GetBCryptPasswordHasher();
        var useCase = new SignInQueryHandler(repository, passwordHasher, jwtTokenGenerator);

        var query = fixture.GetQuery(expectedUser.Email, password);
        var output = await useCase.Handle(query, CancellationToken.None);

        output.ShouldNotBeNull();
        output.Name.ShouldBe(expectedUser.Name);
        output.Email.ShouldBe(expectedUser.Email);
        output.Id.ShouldBe(expectedUser.Id);
        output.Token.ShouldNotBeNull();
    }

    [Fact]
    public async Task Given_SignInQuery_When_ExecuteHandlerWithInexistingUser_Then_ShouldReturnNotFoundException()
    {
        var dbContext = fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var jwtTokenGenerator = fixture.GetJwtTokenGenerator();
        var passwordHasher = fixture.GetBCryptPasswordHasher();
        var useCase = new SignInQueryHandler(repository, passwordHasher, jwtTokenGenerator);

        var query = fixture.GetQuery(fixture.GetValidUserEmail(), fixture.GetValidUserPassword());

        Func<Task<AuthResult>> Action = async () =>
            await useCase.Handle(query, CancellationToken.None);
        var exception = await Should.ThrowAsync<NotFoundException>(Action());
        exception.Message.ShouldBe("User not found.");
    }

    [Fact]
    public async Task Given_SignInQuery_When_ExecuteHandlerWithEistentUserButWrongPassword_Then_ShouldReturnDivergentDataException()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleUser = fixture.GetValidUser();
        await dbContext.AddAsync(exampleUser, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var repository = new UserRepository(dbContext);
        var jwtTokenGenerator = fixture.GetJwtTokenGenerator();
        var passwordHasher = fixture.GetBCryptPasswordHasher();
        var useCase = new SignInQueryHandler(repository, passwordHasher, jwtTokenGenerator);

        var query = fixture.GetQuery(exampleUser.Email, fixture.GetValidUserPassword());

        Func<Task<AuthResult>> Action = async () =>
            await useCase.Handle(query, CancellationToken.None);
        var exception = await Should.ThrowAsync<DivergentDataException>(Action());
        exception.Message.ShouldBe("Wrong password.");
    }
}
