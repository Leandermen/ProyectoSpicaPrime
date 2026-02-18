using api.Application.Abstractions.Persistence.Repositories;
using api.Domain.Agreements;

namespace api.Infrastructure.Persistence.Repositories;

public sealed class AgreementTemplateRepository : IAgreementTemplateRepository
{
    public Task<AgreementTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult<AgreementTemplate?>(null);

    public Task<AgreementTemplate?> GetActiveByServiceIdAsync(Guid serviceId, CancellationToken ct = default)
        => Task.FromResult<AgreementTemplate?>(null);

    public Task<int> GetLastVersionByServiceIdAsync(Guid serviceId, CancellationToken ct = default)
        => Task.FromResult(0);

    public Task AddAsync(AgreementTemplate template, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task UpdateAsync(AgreementTemplate template, CancellationToken ct = default)
        => Task.CompletedTask;
}

