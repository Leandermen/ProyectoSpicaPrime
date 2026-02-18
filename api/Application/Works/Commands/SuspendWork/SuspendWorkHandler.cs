using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Users;

namespace api.Application.Works.Commands.SuspendWork;

public sealed class SuspendWorkHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SuspendWorkHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SuspendWorkResult>> Handle(
        Guid userId,
        SuspendWorkCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<SuspendWorkResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<SuspendWorkResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<SuspendWorkResult>.Fail(Errors.Works.NotFound);

        // 3️⃣ Autorización
        if (user.Id != work.ClientId && user.Id != work.ProfessionalId)
            return Result<SuspendWorkResult>.Fail(Errors.Common.Forbidden);

        // 4️⃣ Validación input
        if (string.IsNullOrWhiteSpace(command.Reason))
            return Result<SuspendWorkResult>.Fail(Errors.Works.EmptyReason);

        // 5️⃣ Dominio
        work.Suspend(command.Reason.Trim());

        // 6️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7️⃣ Resultado
        return Result<SuspendWorkResult>.Ok(
            new SuspendWorkResult(
                work.Id,
                work.Status
            ));
    }
}
