using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Works.Commands.CompleteWork;

public sealed class CompleteWorkHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteWorkHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CompleteWorkResult>> Handle(
        Guid userId,
        CompleteWorkCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<CompleteWorkResult>.Fail(Errors.Users.NotFound);           

        if (!user.IsActive)
            return Result<CompleteWorkResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<CompleteWorkResult>.Fail(Errors.Works.NotFound);

        // 3️⃣ Autorización
        // Solo el profesional puede cerrar el trabajo
        if (user.Id != work.ProfessionalId)
            return Result<CompleteWorkResult>.Fail(Errors.Common.Forbidden);

        // 4️⃣ Dominio
        work.Complete();

        // 5️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CompleteWorkResult>.Ok(
            new CompleteWorkResult(
                work.Id,
                work.CompletedAt!.Value
            ));
    }
}
