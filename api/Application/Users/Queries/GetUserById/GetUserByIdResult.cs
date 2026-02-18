using System;

namespace api.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdResult(
    Guid Id,
    string Email,
    string FullName,
    bool IsActive,
    IReadOnlyList<string> Roles
);

