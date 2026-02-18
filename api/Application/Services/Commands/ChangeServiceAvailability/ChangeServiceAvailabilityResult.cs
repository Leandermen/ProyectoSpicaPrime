namespace api.Application.Services.Commands.ChangeServiceAvailability;

public sealed record ChangeServiceAvailabilityResult(
    Guid ServiceId,
    bool IsAvailable
);
