namespace api.Application.Users.Queries.GetUsers;

public sealed record GetUsersResult(
    Guid Id,
    string Email,
    string FullName,
    bool IsActive,
    IReadOnlyList<string> Roles
);

