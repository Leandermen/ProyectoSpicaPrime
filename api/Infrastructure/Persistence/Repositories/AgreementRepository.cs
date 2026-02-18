using api.Application.Abstractions.Persistence.Repositories;
using api.Domain.Agreements;

namespace api.Infrastructure.Persistence.Repositories;

public sealed class AgreementRepository : IAgreementRepository
{
    public Task<Agreement?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult<Agreement?>(null);

    public Task AddAsync(Agreement agreement, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task UpdateAsync(Agreement agreement, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(false);
}
