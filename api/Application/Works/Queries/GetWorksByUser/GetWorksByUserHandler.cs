using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Works.Queries.GetWorksByUser;

public sealed class GetWorksByUserHandler
{
    private readonly IWorkRepository _workRepository;
    private readonly IUserRepository _userRepository;

    public GetWorksByUserHandler(
        IWorkRepository workRepository,
        IUserRepository userRepository)
    {
        _workRepository = workRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyList<GetWorksByUserResult>>> Handle(
        GetWorksByUserQuery query,
        CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.ExistsAsync(query.UserId, cancellationToken);

        if (!userExists)
            return Result<IReadOnlyList<GetWorksByUserResult>>.Fail(Errors.Users.NotFound);

        var works = await _workRepository.GetByUserAsync(
            query.UserId,
            cancellationToken);

        var result = works
            .Select(w => new GetWorksByUserResult(
                w.Id,
                w.ServiceId,
                w.Status,
                w.StartedAt,
                w.CompletedAt
            ))
            .ToList();

        // Lista vacía es válida
        return Result<IReadOnlyList<GetWorksByUserResult>>.Ok(result);
    }
}
