using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;
using api.Domain.Works;

namespace api.Domain.Works
{
    public sealed class WorkNote : BaseEntity
    {
        public Guid WorkId { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Content { get; private set; } = default!;

        private WorkNote() { }

        public WorkNote(Guid workId, Guid authorId, string content)
        {
            WorkId = workId;
            AuthorId = authorId;
            Content = content;
        }
    }

}