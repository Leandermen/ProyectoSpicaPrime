using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;

namespace api.Domain.Agreements;

public class Agreement : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public Guid AgreementTemplateId { get; private set; }
    public Guid ClientId { get; private set; }

    public string SnapshotContent { get; private set; } = default!;
    public AgreementStatus Status { get; private set; }

    //public DateTime CreatedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }

    protected Agreement() { }

    public Agreement(
        Guid serviceId,
        Guid agreementTemplateId,
        Guid clientId,
        string snapshotContent)
    {
        if (string.IsNullOrWhiteSpace(snapshotContent))
            throw new ArgumentException("El contrato debe tener contenido.");

        ServiceId = serviceId;
        AgreementTemplateId = agreementTemplateId;
        ClientId = clientId;
        SnapshotContent = snapshotContent;

        Status = AgreementStatus.Pending;
        //CreatedAt = DateTime.UtcNow;
    }

    public void Accept()
    {
        if (Status != AgreementStatus.Pending)
            throw new InvalidOperationException("El contrato no puede ser aceptado en su estado actual.");

        Status = AgreementStatus.Accepted;
        AcceptedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status != AgreementStatus.Pending)
            throw new InvalidOperationException("El contrato no puede ser rechazado en su estado actual.");

        Status = AgreementStatus.Rejected;
    }
}
