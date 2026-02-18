namespace api.Application.Services.Commands.CreateService;

public sealed record CreateServiceResult(
    Guid ServiceId,
    string Name,
    bool IsAvailable,
    bool IsActive
);
