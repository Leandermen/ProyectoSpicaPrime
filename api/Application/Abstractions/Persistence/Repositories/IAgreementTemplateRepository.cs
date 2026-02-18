using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Agreements;

namespace api.Application.Abstractions.Persistence.Repositories
{
    public interface IAgreementTemplateRepository
    {
        Task<AgreementTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<AgreementTemplate?> GetActiveByServiceIdAsync(Guid serviceId, CancellationToken ct = default);
        Task<int> GetLastVersionByServiceIdAsync(Guid serviceId, CancellationToken ct = default);
        Task AddAsync(AgreementTemplate template, CancellationToken ct = default);
        Task UpdateAsync(AgreementTemplate template, CancellationToken ct = default);
    }
}