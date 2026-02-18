using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;

namespace api.Domain.Services
{
    public sealed class ServiceRequirement : BaseEntity
    {
        public Guid ServiceId { get; private set; }
        public Service Service { get; private set; } = default!;

        public string Description { get; private set; } = default!;
        public bool IsMandatory { get; private set; }

        private ServiceRequirement() { } // EF Core

        public ServiceRequirement(
            Guid serviceId,
            string description,
            bool isMandatory
        )
        {
            ServiceId = serviceId;
            Description = description;
            IsMandatory = isMandatory;
        }

        public void Update(string description, bool isMandatory)
        {
            Description = description;
            IsMandatory = isMandatory;
        }
    }
}