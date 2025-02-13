using MyMoney.Domain.Entities;

namespace MyMoney.Domain.Repository;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
}
