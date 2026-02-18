namespace api.Application.Works.Commands.SuspendWork;

public sealed record SuspendWorkCommand(
    Guid WorkId,
    string Reason
);
