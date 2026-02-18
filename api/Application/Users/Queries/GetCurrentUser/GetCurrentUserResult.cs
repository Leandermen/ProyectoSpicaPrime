using System;

namespace api.Application.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserResult(
    Guid Id,
    string Email,
    string FullName,
    IReadOnlyCollection<string> Roles,
    bool IsActive
);

