using DomainEntity = MyMoney.Domain.Entities;

namespace MyMoney.UnitTests.Domain.Entities.User;

[Collection(nameof(UserTestFixture))]
public class UserTest(UserTestFixture fixture)
{
    private readonly UserTestFixture _fixture = fixture;

    [Fact]
    public void Given_User_When_CorrectDataIsPassed_Then_ShouldInstantiate()
    {
        (string expectedName, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(expectedPassword);

        var user = new DomainEntity.User(
            expectedName,
            expectedEmail,
            expectedPassword,
            passwordHasherMock.Object
        );

        user.ShouldNotBeNull();
        user.Name.ShouldBe(expectedName);
        user.Email.ShouldBe(expectedEmail);
        user.Password.ShouldBe(expectedPassword);
        user.Id.ShouldNotBe(default);
        user.CreatedAt.ShouldNotBe(default);
        user.LastUpdatedAt.ShouldBeNull();
    }

    [Fact]
    public void Given_User_When_ProvidedCorrectNameToUpdate_Then_ShouldUpdate()
    {
        (string expectedName, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(expectedPassword);

        var user = new DomainEntity.User(
            expectedName,
            expectedEmail,
            expectedPassword,
            passwordHasherMock.Object
        );

        user.LastUpdatedAt.ShouldBeNull();

        var expectedUpdatedName = _fixture.GetValidUserName();
        user.UpdateName(expectedUpdatedName);

        user.ShouldNotBeNull();
        user.Name.ShouldBe(expectedUpdatedName);
        user.Email.ShouldBe(expectedEmail);
        user.Password.ShouldBe(expectedPassword);
        user.Id.ShouldNotBe(default);
        user.CreatedAt.ShouldNotBe(default);
        user.LastUpdatedAt.ShouldNotBeNull(default);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Given_User_When_ProvidedNameAsNullOrEmptyToUpdate_Then_ShouldNotUpdate(
        string incorrectName
    )
    {
        (string expectedName, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(expectedPassword);

        var user = new DomainEntity.User(
            expectedName,
            expectedEmail,
            expectedPassword,
            passwordHasherMock.Object
        );

        user.LastUpdatedAt.ShouldBeNull();
        user.ShouldNotBeNull();

        var action = () => user.UpdateName(incorrectName);

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Name should not be null or empty.");
    }

    [Fact]
    public void Given_User_When_ProvidedNameGreatherThan100CharacterLongToUpdate_Then_ShouldNotUpdate()
    {
        (string expectedName, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(expectedPassword);

        var user = new DomainEntity.User(
            expectedName,
            expectedEmail,
            expectedPassword,
            passwordHasherMock.Object
        );

        user.LastUpdatedAt.ShouldBeNull();
        user.ShouldNotBeNull();
        var incorrectName = _fixture.Faker.Lorem.Sentence(101);

        var action = () => user.UpdateName(incorrectName);

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Name should be less or equal to 100 characters long.");
    }

    [Fact]
    public void Given_User_When_ProvidedNameLessThan3CharacterLongToUpdate_Then_ShouldNotUpdate()
    {
        var (expectedName, expectedEmail, expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(expectedPassword);

        var user = new DomainEntity.User(
            expectedName,
            expectedEmail,
            expectedPassword,
            passwordHasherMock.Object
        );

        user.LastUpdatedAt.ShouldBeNull();
        user.ShouldNotBeNull();
        var incorrectName = expectedName[..3];

        var action = () => user.UpdateName(incorrectName);

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Name should be at least 5 characters long.");
    }

    [Fact]
    public void Given_User_When_ProvidedCorrectPasswordToUpdate_Then_ShouldUpdate()
    {
        (string expectedName, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(expectedPassword);

        var user = new DomainEntity.User(
            expectedName,
            expectedEmail,
            expectedPassword,
            passwordHasherMock.Object
        );

        user.LastUpdatedAt.ShouldBeNull();

        var expectedUpdatedPassword = _fixture.GetValidUserPassword();
        passwordHasherMock
            .Setup(x => x.HashPassword(It.IsAny<string>()))
            .Returns(expectedUpdatedPassword);
        user.UpdatePassword(expectedUpdatedPassword, passwordHasherMock.Object);

        user.ShouldNotBeNull();
        user.Name.ShouldBe(expectedName);
        user.Email.ShouldBe(expectedEmail);
        user.Password.ShouldBe(expectedUpdatedPassword);
        user.Id.ShouldNotBe(default);
        user.CreatedAt.ShouldNotBe(default);
        user.LastUpdatedAt.ShouldNotBeNull(default);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Given_User_When_ProvidedWrongPassword_Then_ShouldNotInstantiate(string? password)
    {
        (string expectedName, string expectedEmail, _) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();

        var action = () =>
            new DomainEntity.User(
                expectedName,
                expectedEmail,
                password!,
                passwordHasherMock.Object
            );

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Password should not be null or empty.");
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("1234")]
    public void Given_User_When_ProvidedWrongPasswordSize_Then_ShouldNotInstantiate(string password)
    {
        (string expectedName, string expectedEmail, _) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();

        var action = () =>
            new DomainEntity.User(expectedName, expectedEmail, password, passwordHasherMock.Object);

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Password should be at least 8 characters long.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Given_User_When_ProvidedNameAsNull_Then_ShouldNotInstantiate(string? name)
    {
        (_, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();

        var action = () =>
            new DomainEntity.User(
                name!,
                expectedEmail,
                expectedPassword,
                passwordHasherMock.Object
            );

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Name should not be null or empty.");
    }

    [Fact]
    public void Given_User_When_ProvidedNameGreaterThan100CharacterLong_Then_ShouldNotInstantiate()
    {
        (_, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        var incorrectName = _fixture.Faker.Lorem.Sentence(range: 101);

        var action = () =>
            new DomainEntity.User(
                incorrectName,
                expectedEmail,
                expectedPassword,
                passwordHasherMock.Object
            );

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Name should be less or equal to 100 characters long.");
    }

    [Fact]
    public void Given_User_When_NameIsLessThan3CharactersLong_Then_ShouldNotInstantiate()
    {
        (string expectedName, string expectedEmail, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();
        var incorrectName = expectedName[..3];

        var action = () =>
            new DomainEntity.User(
                incorrectName,
                expectedEmail,
                expectedPassword,
                passwordHasherMock.Object
            );

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Name should be at least 5 characters long.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void Given_User_When_ProvidedEmailAsNull_Then_ShouldNotInstantiate(string? email)
    {
        (string expectedName, _, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();

        var action = () =>
            new DomainEntity.User(
                expectedName,
                email!,
                expectedPassword,
                passwordHasherMock.Object
            );

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Email should not be null or empty.");
    }

    [Theory]
    [InlineData("test@test.")]
    [InlineData("teste@test")]
    [InlineData("test@")]
    public void Given_User_When_ProvidedInvalidEmail_Then_ShouldNotInstantiate(string? email)
    {
        (string expectedName, _, string expectedPassword) = _fixture.GetValidUserData();
        var passwordHasherMock = _fixture.GetPasswordHasherMock();

        var action = () =>
            new DomainEntity.User(
                expectedName,
                email!,
                expectedPassword,
                passwordHasherMock.Object
            );

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe("Email is not a valid email address.");
    }
}
