using api.Domain.Works;

namespace api.Application.Works.Commands.CreateWorkFromAgreement;

public sealed record CreateWorkFromAgreementResult(
    Guid WorkId,
    Guid AgreementId,
    WorkStatus Status
);
