namespace api.Application.Services.Commands.RequestService;

public sealed record RequestServiceCommand(
    Guid ServiceId,
    string? ClientNotes
);
