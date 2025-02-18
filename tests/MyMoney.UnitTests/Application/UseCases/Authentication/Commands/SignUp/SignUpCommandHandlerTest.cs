using MyMoney.Application.Exceptions;
using MyMoney.Application.Intefaces;
using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.Application.UseCases.Authentication.Common;
using DomainEntity = MyMoney.Domain.Entities;
using MyMoney.Domain.Repository;

namespace MyMoney.UnitTests.Application.UseCases.Authentication.Commands.SignUp;

[Collection(nameof(SignUpCommandHandlerTestFixture))]
public class SignUpCommandHandlerTest(SignUpCommandHandlerTestFixture fixture)
{
    [Fact]
    public async Task Given_RegisterUser_When_InputIsCorrect_Then_ShouldRegisterUser()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns("fake_jwt");

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        var input = fixture.GetExampleInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Once());
        AssertUnitOfWork(unitOfWork, Times.Once());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Once());

        output.Name.ShouldBe(input.Name);
        output.Email.ShouldBe(input.Email);
        output.Id.ShouldNotBe(Guid.Empty);
        output.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Given_RegisterUser_When_AlreadyExistsAUserWithTheSameEmail_Then_ShouldThrowConflictException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        userRepositoryMock
            .Setup(x => x.FindByEmailAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.GetExistentUser);

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        var input = fixture.GetExampleInput();
        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<ConflictException>(Action());
        exception.Message.ShouldBe("User already exists.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Fact]
    public async Task
        Given_RegisterUser_When_InputNameIsLessThan3CharacterLong_Then_ShouldThrowEntityValidationException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (string name, string email, string password) = fixture.GetValidUserData();
        var input = new SignUpCommand(name[..2], email, password);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Name should be at least 5 characters long.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Fact]
    public async Task
        Given_RegisterUser_When_InputNameIsGreaterThan100CharacterLong_Then_ShouldThrowEntityValidationException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (_, string email, string password) = fixture.GetValidUserData();
        string name = fixture.Faker.Lorem.Sentence(range: 101);
        var input = new SignUpCommand(name, email, password);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Name should be less or equal to 100 characters long.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Given_RegisterUser_When_InputNameNullOrEmpty_Then_ShouldThrowEntityValidationException(
        string incorrectName)
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (_, string email, string password) = fixture.GetValidUserData();

        var input = new SignUpCommand(incorrectName, email, password);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Name should not be null or empty.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Given_RegisterUser_When_EmailIsNullOrEmpty_Then_ShouldThrowEntityValidationException(
        string incorrectEmail)
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (string name, _, string password) = fixture.GetValidUserData();

        var input = new SignUpCommand(name, incorrectEmail, password);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);
        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Email should not be null or empty.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Fact]
    public async Task Given_RegisterUser_When_EmailIsInvalid_Then_ShouldThrowEntityValidationException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (string name, _, string password) = fixture.GetValidUserData();
        const string incorrectEmail = "john@doe";

        var input = new SignUpCommand(name, incorrectEmail, password);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);
        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Email is not a valid email address.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Fact]
    public async Task Given_RegisterUser_When_PasswordHasInvalidSize_Then_ShouldThrowEntityValidationException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (string name, string email, _) = fixture.GetValidUserData();
        const string incorrectPassword = "1234567";

        var input = new SignUpCommand(name, email, incorrectPassword);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);
        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Password should be at least 8 characters long.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Given_RegisterUser_When_PasswordIsNullOrEmpty_Then_ShouldThrowEntityValidationException(
        string incorrectPassword)
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var unitOfWork = fixture.GetUnitOfWorkMock();
        var jwtTokenGeneratorMock = fixture.GetJwtTokenGeneratorMock();
        var passwordHasher = fixture.GetPasswordHasherMock();

        var useCase = new SignUpCommandHandler(
            userRepositoryMock.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            jwtTokenGeneratorMock.Object);

        (string name, string email, _) = fixture.GetValidUserData();
        var input = new SignUpCommand(name, email, incorrectPassword);

        async Task<AuthResult> Action() => await useCase.Handle(input, CancellationToken.None);
        var exception = await Should.ThrowAsync<EntityValidationException>(Action());
        exception.Message.ShouldBe("Password should not be null or empty.");

        AssertUserRepositoryFindByEmail(userRepositoryMock, Times.Once());
        AssertUserRepositoryCreate(userRepositoryMock, Times.Never());
        AssertUnitOfWork(unitOfWork, Times.Never());
        AssertJwtTokenGenerator(jwtTokenGeneratorMock, Times.Never());
    }

    private static void AssertJwtTokenGenerator(Mock<IJwtTokenGenerator> jwtTokenGeneratorMock, Times times)
    {
        jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            times);
    }

    private static void AssertUnitOfWork(Mock<IUnitOfWork> unitOfWork, Times times)
    {
        unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            times);
    }

    private static void AssertUserRepositoryCreate(Mock<IUserRepository> userRepositoryMock, Times times)
    {
        userRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<DomainEntity.User>(),
                It.IsAny<CancellationToken>()),
            times);
    }

    private static void AssertUserRepositoryFindByEmail(Mock<IUserRepository> userRepositoryMock, Times times)
    {
        userRepositoryMock.Verify(
            x => x.FindByEmailAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            times);
    }
}