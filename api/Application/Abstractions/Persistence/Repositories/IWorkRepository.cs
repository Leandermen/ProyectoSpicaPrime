using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Works;

namespace api.Application.Abstractions.Persistence.Repositories
{
    public interface IWorkRepository
    {
        Task<Work?> GetByIdAsync(
            Guid workId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Work work,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Work work,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Guid workId,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Work>> GetByUserAsync(
            Guid userId,
            CancellationToken cancellationToken);
    }
}