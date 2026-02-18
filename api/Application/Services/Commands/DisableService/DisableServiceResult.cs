namespace api.Application.Services.Commands.DisableService;

public sealed record DisableServiceResult(
    Guid ServiceId,
    bool IsActive
);
