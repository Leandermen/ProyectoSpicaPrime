using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserHandler
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetCurrentUserResult>> Handle(
        GetCurrentUserQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            query.UserId,
            cancellationToken);

        if (user is null)
            return Result<GetCurrentUserResult>.Fail(
                Errors.Users.NotFound);

        var result = new GetCurrentUserResult(
            user.Id,
            user.Email,
            user.FullName,
            user.Roles.Select(r => r.ToString()).ToList(),
            user.IsActive
            
        );

        return Result<GetCurrentUserResult>.Ok(result);
    }
}
