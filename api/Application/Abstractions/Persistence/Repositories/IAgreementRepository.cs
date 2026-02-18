using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Agreements;

namespace api.Application.Abstractions.Persistence.Repositories
{
    public interface IAgreementRepository
    {
        Task<Agreement?> GetByIdAsync(
            Guid agreementId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Agreement agreement,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Guid agreementId,
            CancellationToken cancellationToken = default);
        Task UpdateAsync(
        Agreement agreement,
        CancellationToken cancellationToken = default);
    }
}