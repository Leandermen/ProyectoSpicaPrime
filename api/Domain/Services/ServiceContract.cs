using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;

namespace api.Domain.Services
{
    public sealed class ServiceContract : BaseEntity
    {
         // --- Foreign Key obligatoria ---
        public Guid ServiceId { get; private set; }

        // --- Navegación ---
        public Service Service { get; private set; } = default!;

        // --- Datos del contrato ---
        public DateOnly StartDate { get; private set; }
        public DateOnly? EndDate { get; private set; }

        public bool IsActive { get; private set; } = true;

        // Constructor requerido por EF
        private ServiceContract() { }
        
            // Constructor de dominio
        public ServiceContract(Guid serviceId, DateOnly startDate, DateOnly? endDate = null)
        {
            if (serviceId == Guid.Empty)
                throw new ArgumentException("ServiceId es obligatorio.", nameof(serviceId));

            if (endDate.HasValue && endDate < startDate)
                throw new ArgumentException("La fecha de término no puede ser anterior a la fecha de inicio.");

            ServiceId = serviceId;
            StartDate = startDate;
            EndDate = endDate;
        }
            // --- Comportamiento de dominio ---

        public void Terminate(DateOnly endDate)
        {
            if (!IsActive)
                throw new InvalidOperationException("El contrato ya está inactivo.");

            if (endDate < StartDate)
                throw new ArgumentException("La fecha de término no puede ser anterior al inicio.");

            EndDate = endDate;
            IsActive = false;
        }

        public void Reactivate(DateOnly newStartDate)
        {
            if (IsActive)
                throw new InvalidOperationException("El contrato ya está activo.");

            StartDate = newStartDate;
            EndDate = null;
            IsActive = true;
        }
    }
}