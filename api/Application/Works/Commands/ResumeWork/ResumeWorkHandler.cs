using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Works.Commands.ResumeWork;

public sealed class ResumeWorkHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResumeWorkHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ResumeWorkResult>> Handle(
        Guid userId,
        ResumeWorkCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<ResumeWorkResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<ResumeWorkResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<ResumeWorkResult>.Fail(Errors.Works.NotFound);

        // 3️⃣ Autorización
        if (user.Id != work.ClientId && user.Id != work.ProfessionalId)
            return Result<ResumeWorkResult>.Fail(Errors.Common.Forbidden);

        // 4️⃣ Dominio
        work.Resume();

        // 5️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6️⃣ Resultado
        return Result<ResumeWorkResult>.Ok(
            new ResumeWorkResult(
            work.Id,
            work.Status
        ));
    }
}
