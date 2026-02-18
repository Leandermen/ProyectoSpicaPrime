using api.Domain.Works;

namespace api.Application.Works.Commands.CancelWork;

public sealed record CancelWorkResult(
    Guid WorkId,
    WorkStatus Status
);
