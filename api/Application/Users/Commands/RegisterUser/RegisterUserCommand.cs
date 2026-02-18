using api.Domain.Users;

namespace api.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FullName,
    string Password,
    UserRole Role
);
