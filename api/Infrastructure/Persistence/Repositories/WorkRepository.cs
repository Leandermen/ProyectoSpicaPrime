using api.Application.Abstractions.Persistence.Repositories;
using api.Domain.Works;

namespace api.Infrastructure.Persistence.Repositories;

public sealed class WorkRepository : IWorkRepository
{
    public Task<Work?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult<Work?>(null);

    public Task AddAsync(Work work, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task UpdateAsync(Work work, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(false);

    public Task<IReadOnlyList<Work>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<Work>>(Array.Empty<Work>());
}
