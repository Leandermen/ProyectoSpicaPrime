namespace api.Application.Services.Commands.ChangeServiceAvailability;

public sealed record ChangeServiceAvailabilityCommand(
    Guid ServiceId,
    bool IsAvailable
);
