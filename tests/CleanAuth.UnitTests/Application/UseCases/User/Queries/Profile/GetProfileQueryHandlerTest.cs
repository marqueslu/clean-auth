using CleanAuth.Application.Exceptions;
using CleanAuth.Application.UseCases.User.Queries.Profile;
using CleanAuth.Domain.Repository;

namespace CleanAuth.UnitTests.Application.UseCases.User.Queries.Profile;

[Collection(nameof(GetProfileQueryHandlerTestFixture))]
public class GetProfileQueryHandlerTest(GetProfileQueryHandlerTestFixture fixture)
{
    [Fact]
    public async Task Given_GetProfileQuery_When_UserExists_Then_ShouldReturnUserInfo()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var expectedUser = fixture.GetUser();
        var input = fixture.GetExampleInput(expectedUser.Id);

        userRepositoryMock
            .Setup<Task<CleanAuth.Domain.Entities.User?>>(x =>
                x.FindByIdAsync(input.Id, CancellationToken.None)
            )
            .ReturnsAsync(expectedUser);

        var useCase = new GetProfileQueryHandler(userRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        AssertUserRepositoryFindById(userRepositoryMock, Times.Once(), input.Id);
        output.Name.ShouldBe(expectedUser.Name);
        output.Email.ShouldBe(expectedUser.Email);
        output.Id.ShouldBe(expectedUser.Id);
    }

    [Fact]
    public async Task Given_GetProfileQuery_When_UserNotExists_Then_ShouldThrowNotFoundException()
    {
        var userRepositoryMock = fixture.GetUserRepositoryMock();
        var expectedUser = fixture.GetUser();
        var input = fixture.GetExampleInput(expectedUser.Id);

        var useCase = new GetProfileQueryHandler(userRepositoryMock.Object);

        async Task<GetProfileResult> Action() =>
            await useCase.Handle(input, CancellationToken.None);

        var exception = await Should.ThrowAsync<NotFoundException>(Action());
        exception.Message.ShouldBe("User not found.");

        AssertUserRepositoryFindById(userRepositoryMock, Times.Once(), input.Id);
    }

    private static void AssertUserRepositoryFindById(
        Mock<IUserRepository> userRepositoryMock,
        Times times,
        Guid id
    )
    {
        userRepositoryMock.Verify(x => x.FindByIdAsync(id, It.IsAny<CancellationToken>()), times);
    }
}
