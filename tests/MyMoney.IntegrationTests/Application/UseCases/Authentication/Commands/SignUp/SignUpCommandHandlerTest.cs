using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Infrastructure.Common.Persistence;
using MyMoney.Infrastructure.Repositories;

namespace MyMoney.IntegrationTests.Application.UseCases.Authentication.Commands.SignUp;

[Collection(nameof(SignUpTestFixture))]
public class SignUpCommandHandlerTest(SignUpTestFixture fixture)
{
    [Fact]
    public async Task Given_SignUpCommand_When_ExecuteHandlerCorrectly_Then_ShouldCreateUser()
    {
        var dbContext = fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var jwtTokenGenerator = fixture.GetJwtTokenGenerator();
        var passwordHasher = fixture.GetBCryptPasswordHasher();
        var useCase = new SignUpCommandHandler(repository, unitOfWork, passwordHasher, jwtTokenGenerator);

        var command = fixture.GetCommand();
        var output = await useCase.Handle(command, CancellationToken.None);

        var registeredUser = await fixture.CreateDbContext(true).Users.FindAsync(output.Id);
        registeredUser.ShouldNotBeNull();
        registeredUser.Name.ShouldBe(command.Name);
        registeredUser.Email.ShouldBe(command.Email);
        output.CreatedAt.ShouldNotBe(default);
        output.Token.ShouldNotBeNull();
    }

    [Theory]
    [MemberData(
        nameof(SignUpTestDataGenerator.GetInvalidInputs),
        parameters: 5,
        MemberType = typeof(SignUpTestDataGenerator)
    )]
    public async Task Given_SignUpCommand_When_CantInstantiateUser_Then_ShouldThrow(SignUpCommand command,
        string expectedExceptionMessage)
    {
        var dbContext = fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var jwtTokenGenerator = fixture.GetJwtTokenGenerator();
        var passwordHasher = fixture.GetBCryptPasswordHasher();
        var useCase = new SignUpCommandHandler(repository, unitOfWork, passwordHasher, jwtTokenGenerator);

        Func<Task<AuthResult>> Action = async () => await useCase.Handle(command, CancellationToken.None);
        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe(expectedExceptionMessage);
    }
}