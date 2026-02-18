namespace api.Application.Agreements.Commands.AcceptAgreement;

public sealed record AcceptAgreementCommand(
    Guid AgreementId
);
