namespace api.Application.Works.Commands.AddWorkNote;

public sealed record AddWorkNoteCommand(
    Guid WorkId,
    string Content
);
