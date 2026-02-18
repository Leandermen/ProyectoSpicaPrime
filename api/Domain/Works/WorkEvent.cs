using api.Domain.Base;
using api.Domain.Works;

namespace api.Domain.Works
{
    public sealed class WorkEvent : BaseEntity
    {
        public Guid WorkId { get; private set; }
        public WorkEventType Type { get; private set; }
        public string Description { get; private set; } = default!;

        private WorkEvent() { }

        public WorkEvent(Guid workId, WorkEventType type, string description)
        {
            WorkId = workId;
            Type = type;
            Description = description;
        }
    }

}