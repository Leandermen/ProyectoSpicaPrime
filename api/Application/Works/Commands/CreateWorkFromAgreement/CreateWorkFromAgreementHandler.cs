using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Agreements;
using api.Domain.Users;
using api.Domain.Works;

namespace api.Application.Works.Commands.CreateWorkFromAgreement;

public sealed class CreateWorkFromAgreementHandler
{
    private readonly IAgreementRepository _agreementRepository;
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkFromAgreementHandler(
        IAgreementRepository agreementRepository,
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _agreementRepository = agreementRepository;
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateWorkFromAgreementResult>> Handle(
        Guid userId,
        CreateWorkFromAgreementCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario ejecutor
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<CreateWorkFromAgreementResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<CreateWorkFromAgreementResult>.Fail(Errors.Common.InvalidOperation);

        if (!user.HasRole(UserRole.Professional) && !user.HasRole(UserRole.Admin))
            return Result<CreateWorkFromAgreementResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Agreement
        var agreement = await _agreementRepository.GetByIdAsync(
            command.AgreementId,
            cancellationToken);
        if (agreement is null)
            return Result<CreateWorkFromAgreementResult>.Fail(Errors.Agreements.NotFound);

        if (agreement.Status != AgreementStatus.Accepted)
            return Result<CreateWorkFromAgreementResult>.Fail(Errors.Agreements.InvalidState);

        // 3️⃣ Crear Work
        var work = new Work(
            agreement.ServiceId,
            agreement.Id,
            agreement.ClientId,
            user.Id // professional ejecutor
        );

        // 4️⃣ Persistencia
        await _workRepository.AddAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 5️⃣ Resultado
        return Result<CreateWorkFromAgreementResult>.Ok(
            new CreateWorkFromAgreementResult(
                work.Id,
                agreement.Id,
                work.Status
            ));
    }
}
