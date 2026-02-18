using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;
using api.Domain.Works;

namespace api.Domain.Works
{
    public sealed class WorkEvidence : BaseEntity
    {
        public Guid WorkId { get; private set; }
        public string FileName { get; private set; } = default!;
        public string StoragePath { get; private set; } = default!;

        private WorkEvidence() { }

        public WorkEvidence(Guid workId, string fileName, string storagePath)
        {
            WorkId = workId;
            FileName = fileName;
            StoragePath = storagePath;
        }
    }

}