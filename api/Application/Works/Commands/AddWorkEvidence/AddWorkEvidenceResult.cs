namespace api.Application.Works.Commands.AddWorkEvidence;

public sealed record AddWorkEvidenceResult(
    Guid WorkId,
    Guid EvidenceId
);
