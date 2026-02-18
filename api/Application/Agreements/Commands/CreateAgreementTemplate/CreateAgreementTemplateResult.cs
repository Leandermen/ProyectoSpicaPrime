using api.Domain.Agreements;

namespace api.Application.Agreements.Commands.CreateAgreementTemplate;

public record CreateAgreementTemplateResult(
    Guid AgreementTemplateId,
    Guid ServiceId,
    int Version,
    AgreementTemplateStatus Status
);
