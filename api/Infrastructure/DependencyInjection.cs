using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Infrastructure.Persistence;
using api.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IWorkRepository, WorkRepository>();
        services.AddScoped<IAgreementRepository, AgreementRepository>();
        services.AddScoped<IAgreementTemplateRepository, AgreementTemplateRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

