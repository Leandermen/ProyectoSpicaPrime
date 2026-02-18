using api.Domain.Agreements;

namespace api.Application.Agreements.Commands.AcceptAgreement;

public sealed record AcceptAgreementResult(
    Guid AgreementId,
    AgreementStatus Status,
    DateTime AcceptedAt
);
