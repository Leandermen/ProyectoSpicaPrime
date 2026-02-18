using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Users.Queries.GetUsers;

public sealed class GetUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyList<GetUsersResult>>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        var result = users
            .Select(u => new GetUsersResult(
                u.Id,
                u.Email,
                u.FullName,
                u.IsActive,
                u.Roles.Select(r => r.ToString()).ToList()
            ))
            .ToList();

        return Result<IReadOnlyList<GetUsersResult>>.Ok(result);
    }
}
