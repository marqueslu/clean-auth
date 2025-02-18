using CleanAuth.Application.Exceptions;
using CleanAuth.Application.UseCases.User.Queries.Profile;
using CleanAuth.Infrastructure.Repositories;

namespace CleanAuth.IntegrationTests.Application.UseCases.User.Queries.Profile;

[Collection(nameof(GetProfileTestFixture))]
public class SignInCommandHandlerTest(GetProfileTestFixture fixture)
{
    [Fact]
    public async Task Given_GetProfileQuery_When_ExecuteHandlerWithCorrectData_Then_ShouldReturnUserInformation()
    {
        var dbContext = fixture.CreateDbContext();
        var expectedUser = fixture.GetValidUser();
        await dbContext.AddAsync(expectedUser, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var repository = new UserRepository(dbContext);
        var useCase = new GetProfileQueryHandler(repository);

        var query = fixture.GetQuery(expectedUser.Id);
        var output = await useCase.Handle(query, CancellationToken.None);

        output.ShouldNotBeNull();
        output.Name.ShouldBe(expectedUser.Name);
        output.Email.ShouldBe(expectedUser.Email);
        output.Id.ShouldBe(expectedUser.Id);
    }

    [Fact]
    public async Task Given_SignInQuery_When_ExecuteHandlerWithInexistingUser_Then_ShouldReturnNotFoundException()
    {
        var dbContext = fixture.CreateDbContext();
        var repository = new UserRepository(dbContext);
        var useCase = new GetProfileQueryHandler(repository);

        var query = fixture.GetQuery(Guid.NewGuid());

        Func<Task<GetProfileResult>> Action = async () =>
            await useCase.Handle(query, CancellationToken.None);
        var exception = await Should.ThrowAsync<NotFoundException>(Action());
        exception.Message.ShouldBe("User not found.");
    }
}
