using api.Domain.Works;

namespace api.Application.Works.Commands.SuspendWork;

public sealed record SuspendWorkResult(
    Guid WorkId,
    WorkStatus Status
);
