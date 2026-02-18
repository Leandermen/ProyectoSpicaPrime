using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;

namespace api.Domain.Services
{
    public class Service : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public int EstimatedDurationDays { get; private set; }
        public bool IsAvailable { get; private set; } = true;
        public bool IsActive { get; private set; } = true;

        public ICollection<ServiceRequirement> Requirements { get; private set; } = new List<ServiceRequirement>();
        public ICollection<ServiceContract> Contracts { get; private set; } = new List<ServiceContract>();

        protected Service() { }

        public Service(string name, string description, int estimatedDurationDays)
        {
            Name = name;
            Description = description;
            EstimatedDurationDays = estimatedDurationDays;
        }

        public void SetAvailability(bool available)
            => IsAvailable = available;

        public void Disable()
            => IsActive = false;
        public void Enable()
            => IsActive = true;
    }
}