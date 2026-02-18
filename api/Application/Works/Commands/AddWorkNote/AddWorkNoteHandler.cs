using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Works.Commands.AddWorkNote;

public sealed class AddWorkNoteHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddWorkNoteHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AddWorkNoteResult>> Handle(
        Guid userId,
        AddWorkNoteCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<AddWorkNoteResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<AddWorkNoteResult>.Fail(Errors.Common.InvalidOperation);

        // 2️⃣ Work
        var work = await _workRepository.GetByIdAsync(command.WorkId, cancellationToken);
        if (work is null)
            return Result<AddWorkNoteResult>.Fail(Errors.Works.NotFound);

        // 3️⃣ Autorización
        if (user.Id != work.ClientId && user.Id != work.ProfessionalId)
            return Result<AddWorkNoteResult>.Fail(Errors.Common.Forbidden);

        // 4️⃣ Dominio
        work.AddNote(user.Id, command.Content);

        var noteId = work.Notes.Last().Id;

        // 5️⃣ Persistencia
        await _workRepository.UpdateAsync(work, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AddWorkNoteResult>.Ok(
            new AddWorkNoteResult(
                work.Id,
                noteId
            ));
    }
}
