using Microsoft.EntityFrameworkCore;

using DomainEntity = MyMoney.Domain.Entities;
using MyMoney.Infrastructure.Common.Persistence;

namespace MyMoney.e2eTests.Api.Authentication.Common;

public class AuthenticationPersistence
{
    private readonly AppDbContext _context;

    public AuthenticationPersistence(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntity.User?> GetById(Guid id)
        => await _context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task Save(DomainEntity.User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}