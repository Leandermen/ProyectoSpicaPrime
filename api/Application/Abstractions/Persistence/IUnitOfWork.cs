using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Abstractions.Persistence.Repositories;

namespace api.Application.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IWorkRepository Works { get; }
        IServiceRepository Services { get; }
        IAgreementTemplateRepository AgreementTemplates { get; }
        IAgreementRepository Agreements { get; }

        Task<int> SaveChangesAsync(
            CancellationToken ct = default);
    }
}