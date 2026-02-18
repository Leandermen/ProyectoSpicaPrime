using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;

namespace api.Domain.Agreements;

public class AgreementTemplate : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public int Version { get; private set; }
    public string Content { get; private set; } = default!;
    public AgreementTemplateStatus Status { get; private set; }

    protected AgreementTemplate() { }

    public AgreementTemplate(
        Guid serviceId,
        int version,
        string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("El contenido del contrato no puede estar vacío.");

        ServiceId = serviceId;
        Version = version;
        Content = content;
        Status = AgreementTemplateStatus.Draft;
    }

    public void Activate()
    {
        if (Status == AgreementTemplateStatus.Archived)
            throw new InvalidOperationException("No se puede activar un contrato archivado.");

        Status = AgreementTemplateStatus.Active;
    }

    public void Archive()
    {
        Status = AgreementTemplateStatus.Archived;
    }
}
