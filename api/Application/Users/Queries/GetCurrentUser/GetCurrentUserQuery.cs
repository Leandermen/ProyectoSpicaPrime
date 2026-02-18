using System;

namespace api.Application.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(Guid UserId);
