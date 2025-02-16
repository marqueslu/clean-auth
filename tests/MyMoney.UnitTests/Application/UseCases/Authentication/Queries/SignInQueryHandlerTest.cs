using MyMoney.Application.Exceptions;
using MyMoney.Application.Intefaces;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Application.UseCases.Authentication.Queries.SignIn;
using MyMoney.Domain.Interfaces.Security;
using MyMoney.Domain.Repository;

namespace MyMoney.UnitTests.Application.UseCases.Authentication.Queries.SignIn;

[Collection(nameof(SignInQueryHandlerTestFixture))]
public class SignInQueryHandlerTest(SignInQueryHandlerTestFixture fixture)
{
    [Fact]
    public async Task Given_SignUser_When_InputIsCorrect_Then_ShouldSignIn()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasherMock = fixture.GetPasswordHasherMock();
        var expectedUser = fixture.GetUser(passwordHasherMock.Object);
        var input = fixture.GetExampleInput(expectedUser.Email, expectedUser.Password);

        jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("fake_jwt");
        userRepositoryMock
            .Setup(x => x.FindByEmailAsync(expectedUser.Email, CancellationToken.None))
            .ReturnsAsync(expectedUser);
        passwordHasherMock
            .Setup(x => x.VerifyPassword(input.Password, expectedUser.Password))
            .Returns(true);

        var useCase = new SignInQueryHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertPasswordHasher(
            passwordHasherMock,
            Times.Once(),
            (input.Password, expectedUser.Password)
        );
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Once());
        output.Name.ShouldBe(expectedUser.Name);
        output.Email.ShouldBe(input.Email);
        output.Id.ShouldBe(expectedUser.Id);
        output.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Given_SignInUser_When_UserNotFound_Then_ShouldThrowNotFoundException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasherMock = fixture.GetPasswordHasherMock();
        var expectedUser = fixture.GetUser(passwordHasherMock.Object);
        var input = fixture.GetExampleInput(expectedUser.Email, expectedUser.Password);

        var useCase = new SignInQueryHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object
        );

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<NotFoundException>(Action());
        exception.Message.ShouldBe("User not found.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertPasswordHasher(
            passwordHasherMock,
            Times.Never(),
            (input.Password, expectedUser.Password)
        );
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Fact]
    public async Task Given_SignInUser_When_WrongPasswordIsInformed_Then_ShouldThrowDivergentDataException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasherMock = fixture.GetPasswordHasherMock();
        var expectedUser = fixture.GetUser(passwordHasherMock.Object);
        var input = fixture.GetExampleInput(expectedUser.Email, "wrongPassword");

        userRepositoryMock
            .Setup(x => x.FindByEmailAsync(expectedUser.Email, CancellationToken.None))
            .ReturnsAsync(expectedUser);
        passwordHasherMock
            .Setup(x => x.VerifyPassword(input.Password, expectedUser.Password))
            .Returns(false);

        var useCase = new SignInQueryHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object
        );

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<DivergentDataException>(Action());
        exception.Message.ShouldBe("Wrong password.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertPasswordHasher(
            passwordHasherMock,
            Times.Once(),
            (input.Password, expectedUser.Password)
        );
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    private static void AssertJwtTokenGenerator(
        Mock<IJwtTokenGenerator> jwtTokenGeneratorMock,
        Times times
    )
    {
        jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
            times
        );
    }

    private static void AssertPasswordHasher(
        Mock<IPasswordHasher> passwordHasherMock,
        Times times,
        (string inputPassword, string hashedPassword) password
    )
    {
        passwordHasherMock.Verify(
            x => x.VerifyPassword(password.inputPassword, password.hashedPassword),
            times
        );
    }

    private static void AssertUserRepositoryFindByEmail(
        Mock<IUserRepository> userRepositoryMock,
        Times times
    )
    {
        userRepositoryMock.Verify(
            x => x.FindByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            times
        );
    }
}
