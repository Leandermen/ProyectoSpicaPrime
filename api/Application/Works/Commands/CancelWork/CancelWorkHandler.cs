using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Works.Commands.CancelWork;

public sealed class CancelWorkHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelWorkHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CancelWorkResult>> Handle(
        Guid userId,
        CancelWorkCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<CancelWorkResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<CancelWorkResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Validación input
        if (string.IsNullOrWhiteSpace(command.Reason))
            return Result<CancelWorkResult>.Fail(Errors.Works.EmptyReason);

        // 3️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<CancelWorkResult>.Fail(Errors.Works.NotFound);

        // 4️⃣ Autorización
        if (user.Id != work.ClientId && user.Id != work.ProfessionalId)
            return Result<CancelWorkResult>.Fail(Errors.Common.Forbidden);

        // 5️⃣ Dominio
        work.Cancel(command.Reason.Trim());

        // 6️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7️⃣ Resultado
        return Result<CancelWorkResult>.Ok(
            new CancelWorkResult(
                work.Id,
                work.Status
            ));
    }
}
