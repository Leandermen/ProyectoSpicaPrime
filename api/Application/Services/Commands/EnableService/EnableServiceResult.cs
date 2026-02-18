namespace api.Application.Services.Commands.EnableService;

public sealed record EnableServiceResult(
    Guid ServiceId,
    bool IsActive
);
