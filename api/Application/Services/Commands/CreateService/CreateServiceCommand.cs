namespace api.Application.Services.Commands.CreateService;

public sealed record CreateServiceCommand(
    Guid UserId,
    string Name,
    string Description,
    int EstimatedDurationDays
);
