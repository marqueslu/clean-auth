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

    [Fact]
    public async Task Given_UserRepository_When_FindByEmailAndUserExists_Then_ShouldReturnUser()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleUser = fixture.GetValidUser();

        await dbContext.AddAsync(exampleUser, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var userRepository = new UserRepository(fixture.CreateDbContext(true));
        var output = await userRepository.FindByEmailAsync(
            exampleUser.Email,
            CancellationToken.None
        );

        output!.Name.ShouldBe(exampleUser.Name);
        output.Email.ShouldBe(exampleUser.Email);
        output.Password.ShouldBe(exampleUser.Password);
        output.CreatedAt.ShouldBe(exampleUser.CreatedAt);
    }

    [Fact]
    public async Task Given_UserRepository_When_FindByEmailAndUserNotExists_Then_ShouldNotReturn()
    {
        var exampleUser = fixture.GetValidUser();
        var dbContext = fixture.CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var output = await userRepository.FindByEmailAsync(
            exampleUser.Email,
            CancellationToken.None
        );

        output.ShouldBeNull();
    }
    
    [Fact]
    public async Task Given_UserRepository_When_FindByIdAndUserExists_Then_ShouldReturnUser()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleUser = fixture.GetValidUser();

        await dbContext.AddAsync(exampleUser, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var userRepository = new UserRepository(fixture.CreateDbContext(true));
        var output = await userRepository.FindByIdAsync(
            exampleUser.Id,
            CancellationToken.None
        );

        output!.Id.ShouldBe(exampleUser.Id);
        output.Name.ShouldBe(exampleUser.Name);
        output.Email.ShouldBe(exampleUser.Email);
        output.Password.ShouldBe(exampleUser.Password);
        output.CreatedAt.ShouldBe(exampleUser.CreatedAt);
    }

    [Fact]
    public async Task Given_UserRepository_When_FindByIdAndUserNotExists_Then_ShouldNotReturn()
    {
        var exampleUser = fixture.GetValidUser();
        var dbContext = fixture.CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var output = await userRepository.FindByIdAsync(
            exampleUser.Id,
            CancellationToken.None
        );

        output.ShouldBeNull();
    }
}
