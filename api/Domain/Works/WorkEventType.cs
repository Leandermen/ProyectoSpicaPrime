namespace api.Domain.Works;

public enum WorkEventType
{
    Created = 1,
    Started = 2,
    Suspended = 3,
    Resumed = 4,
    Completed = 5,
    Cancelled = 6,

    NoteAdded = 10,
    EvidenceAdded = 11,

    SystemEvent = 20
}
