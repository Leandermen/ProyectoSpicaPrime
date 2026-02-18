using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;

namespace api.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    public IUserRepository Users { get; }
    public IWorkRepository Works { get; }
    public IServiceRepository Services { get; }
    public IAgreementTemplateRepository AgreementTemplates { get; }
    public IAgreementRepository Agreements { get; }

    public UnitOfWork(
        IUserRepository users,
        IWorkRepository works,
        IServiceRepository services,
        IAgreementTemplateRepository agreementTemplates,
        IAgreementRepository agreements)
    {
        Users = users;
        Works = works;
        Services = services;
        AgreementTemplates = agreementTemplates;
        Agreements = agreements;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(0);
}
