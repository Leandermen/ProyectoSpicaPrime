using api.Domain.Works;

namespace api.Application.Works.Commands.StartWork;

public sealed record StartWorkResult(
    Guid WorkId,
    WorkStatus Status,
    DateTimeOffset StartedAt
);
