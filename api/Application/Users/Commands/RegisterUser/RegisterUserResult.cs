namespace api.Application.Users.Commands.RegisterUser;

public record RegisterUserResult(
    Guid UserId,
    string Email
);
