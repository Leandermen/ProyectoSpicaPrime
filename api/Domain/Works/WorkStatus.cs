namespace api.Domain.Works
{
    public enum WorkStatus
    {
        Created = 1,     // Work creado desde Agreement aceptado
        InProgress = 2,  // Trabajo en ejecución
        Suspended = 3,   // Pausado temporalmente (bloqueos, info faltante, etc.)
        Completed = 4,   // Ejecutado correctamente
        Cancelled = 5    // Cancelado antes de finalizar
    }
}