namespace api.Application.Agreements.Commands.CreateAgreementTemplate;

public record CreateAgreementTemplateCommand(
    Guid ServiceId,
    string Content
);
