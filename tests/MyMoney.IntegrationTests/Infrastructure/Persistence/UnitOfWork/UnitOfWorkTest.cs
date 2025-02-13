using Microsoft.EntityFrameworkCore;

namespace MyMoney.IntegrationTests.Infrastructure.Persistence.UnitOfWork;

using UnitOfWorkInfra = MyMoney.Infrastructure.Common.Persistence;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest(UnitOfWorkTestFixture fixture)
{
    [Fact]
    public async Task Given_Commit_When_SaveData_Then_ShouldPersistInTheDatabase()
    {
        var dbContext = fixture.CreateDbContext();
        var validUser = fixture.GetValidUser();

        await dbContext.AddAsync(validUser);

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        await unitOfWork.CommitAsync(CancellationToken.None);

        var assertDbContext = fixture.CreateDbContext(true);
        var savedUser = await assertDbContext.Users.FindAsync(validUser.Id);

        savedUser!.Name.ShouldBe(validUser.Name);
        savedUser!.Email.ShouldBe(validUser.Email);
        savedUser!.Password.ShouldBe(validUser.Password);
    }

    [Fact]
    public async Task Given_Rollback_When_Called_Then_ShouldExecute()
    {
        var dbContext = fixture.CreateDbContext();

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        var task = async () => await unitOfWork.RollbackAsync(CancellationToken.None);

        await task.ShouldNotThrowAsync();
    }
}