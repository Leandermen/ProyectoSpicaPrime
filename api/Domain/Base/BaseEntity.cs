using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Domain.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        internal void SetCreated(DateTimeOffset nowUtc)
        {
            CreatedAt = nowUtc;
        }

        internal void SetUpdated(DateTimeOffset nowUtc)
        {
            UpdatedAt = nowUtc;
        }
    }
}