namespace api.Application.Works.Commands.AddWorkNote;

public sealed record AddWorkNoteResult(
    Guid WorkId,
    Guid NoteId
);
