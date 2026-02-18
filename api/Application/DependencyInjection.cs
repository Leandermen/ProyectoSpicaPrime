using Microsoft.Extensions.DependencyInjection;
using api.Application.Services.Queries.GetAvailableServices;
using api.Application.Services.Queries.GetServiceById;
using api.Application.Services.Commands.RequestService;

using api.Application.Works.Commands.StartWork;
using api.Application.Works.Commands.SuspendWork;
using api.Application.Works.Commands.ResumeWork;
using api.Application.Works.Commands.CompleteWork;
using api.Application.Works.Commands.CancelWork;

using api.Application.Works.Queries.GetWorksByUser;
using api.Application.Users.Queries.GetUsers;
using api.Application.Users.Queries.GetUserById;
using api.Application.Users.Queries.GetCurrentUser;

namespace api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetAvailableServicesHandler>();
        services.AddScoped<GetServiceByIdHandler>();
        services.AddScoped<RequestServiceHandler>();

        services.AddScoped<StartWorkHandler>();
        services.AddScoped<SuspendWorkHandler>();
        services.AddScoped<ResumeWorkHandler>();
        services.AddScoped<CompleteWorkHandler>();
        services.AddScoped<CancelWorkHandler>();

        services.AddScoped<GetWorksByUserHandler>();
        services.AddScoped<GetUsersHandler>();
        services.AddScoped<GetUserByIdHandler>();
        services.AddScoped<GetCurrentUserHandler>();

        return services;
    }
}
