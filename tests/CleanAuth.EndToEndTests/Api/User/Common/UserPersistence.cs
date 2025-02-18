using CleanAuth.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using DomainEntity = CleanAuth.Domain.Entities;

namespace CleanAuth.e2eTests.Api.User.Common;

public class UserPersistence
{
    private readonly AppDbContext _context;

    public UserPersistence(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntity.User?> GetById(Guid id) =>
        await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task Save(DomainEntity.User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
