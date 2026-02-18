using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Users;
using api.Domain.Works;

namespace api.Application.Works.Commands.StartWork;

public sealed class StartWorkHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartWorkHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<StartWorkResult>> Handle(
        Guid userId,
        StartWorkCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<StartWorkResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<StartWorkResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<StartWorkResult>.Fail(Errors.Works.NotFound);

        // 3️⃣ Autorización
        if (user.Id != work.ProfessionalId)
            return Result<StartWorkResult>.Fail(Errors.Common.Forbidden);

        // 4️⃣ Dominio
        work.Start();

        // 5️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6️⃣ Resultado
        return Result<StartWorkResult>.Ok(new StartWorkResult(
            work.Id,
            work.Status,
            work.StartedAt!.Value
        ));
    }
}
