using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;
using api.Domain.Users;
using api.Domain.Services;
using api.Domain.Works;

namespace api.Domain.Works;

public sealed class Work : BaseEntity
{
    // --- Relaciones obligatorias ---
    public Guid ServiceId { get; private set; }
    public Guid AgreementId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid ProfessionalId { get; private set; }

    // --- Estado ---
    public WorkStatus Status { get; private set; }

    // --- Auditoría de ejecución ---
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    // --- Bitácora / Comunicación ---
    private readonly List<WorkEvent> _events = new();
    private readonly List<WorkNote> _notes = new();
    private readonly List<WorkEvidence> _evidences = new();

    public IReadOnlyCollection<WorkEvent> Events => _events;
    public IReadOnlyCollection<WorkNote> Notes => _notes;
    public IReadOnlyCollection<WorkEvidence> Evidences => _evidences;

    // Constructor EF
    private Work() { }

    // Constructor de dominio
    public Work(
        Guid serviceId,
        Guid agreementId,
        Guid clientId,
        Guid professionalId)
    {
        if (serviceId == Guid.Empty) throw new ArgumentException("ServiceId es obligatorio.");
        if (agreementId == Guid.Empty) throw new ArgumentException("AgreementId es obligatorio.");
        if (clientId == Guid.Empty) throw new ArgumentException("ClientId es obligatorio.");
        if (professionalId == Guid.Empty) throw new ArgumentException("ProfessionalId es obligatorio.");

        ServiceId = serviceId;
        AgreementId = agreementId;
        ClientId = clientId;
        ProfessionalId = professionalId;

        Status = WorkStatus.Created;
        AddEvent(WorkEventType.Created, "Orden de trabajo creada.");
    }

    // --- Comportamiento de dominio ---
    public void Start()
    {
        EnsureStatus(WorkStatus.Created);

        Status = WorkStatus.InProgress;
        StartedAt = DateTimeOffset.UtcNow;

        AddEvent(WorkEventType.Started, "Trabajo iniciado.");
    }

    public void Suspend(string reason)
    {
        EnsureStatus(WorkStatus.InProgress);

        Status = WorkStatus.Suspended;
        AddEvent(WorkEventType.Suspended, reason);
    }

    public void Resume()
    {
        EnsureStatus(WorkStatus.Suspended);

        Status = WorkStatus.InProgress;
        AddEvent(WorkEventType.Resumed, "Trabajo reanudado.");
    }

    public void Complete()
    {
        EnsureStatus(WorkStatus.InProgress);

        Status = WorkStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;

        AddEvent(WorkEventType.Completed, "Trabajo finalizado.");
    }

    public void Cancel(string reason)
    {
        if (Status == WorkStatus.Completed)
            throw new InvalidOperationException("No se puede cancelar un trabajo completado.");

        Status = WorkStatus.Cancelled;
        AddEvent(WorkEventType.Cancelled, reason);
    }

    // --------------------
    // Información y respaldo
    // --------------------

    public void AddNote(Guid authorId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("La nota no puede estar vacía.");

        var note = new WorkNote(Id, authorId, content);
        _notes.Add(note);

        AddEvent(WorkEventType.NoteAdded, "Nota agregada.");
    }

    public void AddEvidence(string fileName, string storagePath)
    {
        var evidence = new WorkEvidence(Id, fileName, storagePath);
        _evidences.Add(evidence);

        AddEvent(WorkEventType.EvidenceAdded, $"Evidencia agregada: {fileName}");
    }

    // --------------------
    // Helpers privados
    // --------------------

    private void AddEvent(WorkEventType type, string description)
    {
        _events.Add(new WorkEvent(Id, type, description));
    }

    private void EnsureStatus(WorkStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException(
                $"La operación no es válida cuando el trabajo está en estado {Status}.");
    }
}
