namespace api.Application.Agreements.Commands.ActivateAgreementTemplate;

public sealed record ActivateAgreementTemplateCommand(
    Guid AgreementTemplateId
);