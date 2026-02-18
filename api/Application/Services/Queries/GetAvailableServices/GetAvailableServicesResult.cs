namespace api.Application.Services.Queries.GetAvailableServices;

public sealed record GetAvailableServicesResult(
    Guid ServiceId,
    string Name,
    string Description
);
