using api.Domain.Agreements;

namespace api.Application.Services.Commands.RequestService;

public sealed record RequestServiceResult(
    Guid AgreementId,
    Guid ServiceId,
    Guid ClientId,
    AgreementStatus Status
);
