using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Agreements;
using api.Domain.Users;

namespace api.Application.Agreements.Commands.AcceptAgreement;

public sealed class AcceptAgreementHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IAgreementRepository _agreementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptAgreementHandler(
        IUserRepository userRepository,
        IAgreementRepository agreementRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _agreementRepository = agreementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AcceptAgreementResult>> Handle(
        Guid userId,
        AcceptAgreementCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<AcceptAgreementResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<AcceptAgreementResult>.Fail(Errors.Users.Inactive);

        if (!user.HasRole(UserRole.Client))
            return Result<AcceptAgreementResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Agreement
        var agreement = await _agreementRepository.GetByIdAsync(
            command.AgreementId,
            cancellationToken);
        if (agreement is null)
            return Result<AcceptAgreementResult>.Fail(Errors.Agreements.NotFound);

        // 3️⃣ Autorización
        if (agreement.ClientId != user.Id)
            return Result<AcceptAgreementResult>.Fail(Errors.Agreements.InvalidState);

        // 4️⃣ Dominio
        agreement.Accept();

        // 5️⃣ Persistencia
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6️⃣ Resultado
        return Result<AcceptAgreementResult>.Ok(
            new AcceptAgreementResult(
            agreement.Id,
            agreement.Status,
            agreement.AcceptedAt!.Value
        ));
    }
}
