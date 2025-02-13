using Microsoft.EntityFrameworkCore;

using MyMoney.Domain.Entities;
using MyMoney.Domain.Repository;
using MyMoney.Infrastructure.Common.Persistence;

namespace MyMoney.Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task CreateAsync(User user, CancellationToken cancellationToken) =>
        await dbContext.AddAsync(user, cancellationToken);

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken) =>
        await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.Users.FindAsync(id, cancellationToken);

    public Task UpdateAsync(User user, CancellationToken cancellationToken) =>
        Task.FromResult(dbContext.Update(user));
}