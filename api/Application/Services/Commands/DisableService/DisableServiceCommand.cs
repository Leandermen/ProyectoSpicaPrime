namespace api.Application.Services.Commands.DisableService;

public sealed record DisableServiceCommand(
    Guid ServiceId
);
