using api.Domain.Works;

namespace api.Application.Works.Queries.GetWorksByUser;

public sealed record GetWorksByUserResult(
    Guid WorkId,
    Guid ServiceId,
    WorkStatus Status,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt
);

