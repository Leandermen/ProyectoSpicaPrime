using api.Domain.Works;

namespace api.Application.Works.Commands.ResumeWork;

public sealed record ResumeWorkResult(
    Guid WorkId,
    WorkStatus Status
);
