using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Works.Commands.AddWorkEvidence;

public sealed class AddWorkEvidenceHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddWorkEvidenceHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AddWorkEvidenceResult>> Handle(
        Guid userId,
        AddWorkEvidenceCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<AddWorkEvidenceResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<AddWorkEvidenceResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<AddWorkEvidenceResult>.Fail(Errors.Works.NotFound);

        // 3️⃣ Autorización
        if (user.Id != work.ClientId && user.Id != work.ProfessionalId)
            return Result<AddWorkEvidenceResult>.Fail(Errors.Common.Forbidden);

        // 4️⃣ Dominio
        work.AddEvidence(command.FileName, command.StoragePath);

        var evidenceId = work.Evidences.Last().Id;

        // 5️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AddWorkEvidenceResult>.Ok(
            new AddWorkEvidenceResult(
                work.Id, 
                evidenceId
            ));
    }
}
