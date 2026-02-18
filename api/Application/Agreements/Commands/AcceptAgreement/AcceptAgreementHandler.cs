using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
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

    public async Task<AcceptAgreementResult> Handle(
        Guid userId,
        AcceptAgreementCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("El usuario no existe.");

        if (!user.IsActive)
            throw new InvalidOperationException("El usuario no está activo.");

        if (!user.HasRole(UserRole.Client))
            throw new InvalidOperationException("El usuario no puede aceptar contratos.");

        // 2️⃣ Agreement
        var agreement = await _agreementRepository.GetByIdAsync(
            command.AgreementId,
            cancellationToken)
            ?? throw new InvalidOperationException("El contrato no existe.");

        // 3️⃣ Autorización
        if (agreement.ClientId != user.Id)
            throw new InvalidOperationException(
                "El contrato no pertenece al usuario.");

        // 4️⃣ Dominio
        agreement.Accept();

        // 5️⃣ Persistencia
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6️⃣ Resultado
        return new AcceptAgreementResult(
            agreement.Id,
            agreement.Status,
            agreement.AcceptedAt!.Value
        );
    }
}
