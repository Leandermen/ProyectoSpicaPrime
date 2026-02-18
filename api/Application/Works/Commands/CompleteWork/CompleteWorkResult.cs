namespace api.Application.Works.Commands.CompleteWork;

public sealed record CompleteWorkResult(
    Guid WorkId,
    DateTimeOffset CompletedAt
);
