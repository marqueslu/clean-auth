using Microsoft.EntityFrameworkCore;
using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Repository;
using CleanAuth.Infrastructure.Common.Persistence;

namespace CleanAuth.Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task CreateAsync(User user, CancellationToken cancellationToken) =>
        await dbContext.AddAsync(user, cancellationToken);

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken) =>
        await dbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.Users.FindAsync(id, cancellationToken);

    public Task UpdateAsync(User user, CancellationToken cancellationToken) =>
        Task.FromResult(dbContext.Update(user));
}
