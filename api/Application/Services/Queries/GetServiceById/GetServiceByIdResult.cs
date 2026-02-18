namespace api.Application.Services.Queries.GetServiceById;

public sealed record GetServiceByIdResult(
    Guid Id,
    string Name,
    string Description,
    bool IsActive
);
