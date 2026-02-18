using api.Application.Abstractions.Persistence.Repositories;
using api.Domain.Users;

namespace api.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult<User?>(null);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => Task.FromResult<User?>(null);

    public Task AddAsync(User user, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<User>>(Array.Empty<User>());

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(false);
}
