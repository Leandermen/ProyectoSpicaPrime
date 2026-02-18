using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserByIdResult>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            query.UserId,
            cancellationToken);

        if (user is null)
            return Result<GetUserByIdResult>.Fail(
                Errors.Users.NotFound);

        var result = new GetUserByIdResult(
            user.Id,
            user.Email,
            user.FullName,
            user.IsActive,
            user.Roles.Select(r => r.ToString()).ToList()
        );

        return Result<GetUserByIdResult>.Ok(result);
    }
}
