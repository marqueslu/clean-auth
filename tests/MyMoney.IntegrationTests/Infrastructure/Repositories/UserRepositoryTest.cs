using MyMoney.Infrastructure.Repositories;

namespace MyMoney.IntegrationTests.Infrastructure.Repositories;

[Collection(nameof(UserRepositoryTestFixture))]
public class UserRepositoryTest(UserRepositoryTestFixture fixture)
{
    [Fact]
    public async Task Given_UserRepository_When_CreateAsync_Then_ShouldCreateWithSuccess()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleUser = fixture.GetValidUser();

        var userRepository = new UserRepository(dbContext);

        await userRepository.CreateAsync(exampleUser, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var savedDbUser = await fixture.CreateDbContext(true).Users.FindAsync(exampleUser.Id);
        savedDbUser!.Name.ShouldBe(exampleUser.Name);
        savedDbUser.Email.ShouldBe(exampleUser.Email);
        savedDbUser.Password.ShouldBe(exampleUser.Password);
        savedDbUser.CreatedAt.ShouldBe(exampleUser.CreatedAt);
    }
}