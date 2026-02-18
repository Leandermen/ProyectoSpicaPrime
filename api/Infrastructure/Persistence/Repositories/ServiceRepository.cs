using api.Application.Abstractions.Persistence.Repositories;
using api.Domain.Services;

namespace api.Infrastructure.Persistence.Repositories;

public sealed class ServiceRepository : IServiceRepository
{
    public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult<Service?>(null);

    public Task AddAsync(Service service, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(false);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => Task.FromResult(false);

    public Task<IReadOnlyList<Service>> GetAvailableAsync(CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<Service>>(Array.Empty<Service>());

    public Task<IReadOnlyList<Service>> GetByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<Service>>(Array.Empty<Service>());
}
