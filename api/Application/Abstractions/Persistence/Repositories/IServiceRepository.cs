using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Services;

namespace api.Application.Abstractions.Persistence.Repositories
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(
            Guid serviceId,
            CancellationToken cancellationToken = default);
        Task AddAsync(
            Service service,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(
            Guid serviceId,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Service>> GetAvailableAsync(
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Service>> GetByProfessionalIdAsync(
            Guid professionalId,
            CancellationToken ct);
    }
}