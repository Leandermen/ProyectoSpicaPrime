namespace api.Application.Works.Commands.CancelWork;

public sealed record CancelWorkCommand(
    Guid WorkId,
    string Reason
);
