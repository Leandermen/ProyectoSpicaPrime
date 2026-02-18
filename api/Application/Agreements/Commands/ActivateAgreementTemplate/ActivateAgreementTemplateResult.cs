using api.Domain.Agreements;

namespace api.Application.Agreements.Commands.ActivateAgreementTemplate;

public sealed record ActivateAgreementTemplateResult(
    Guid AgreementTemplateId,
    AgreementTemplateStatus Status
);