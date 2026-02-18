namespace api.Application.Works.Commands.AddWorkEvidence;

public sealed record AddWorkEvidenceCommand(
    Guid WorkId,
    string FileName,
    string StoragePath
);
