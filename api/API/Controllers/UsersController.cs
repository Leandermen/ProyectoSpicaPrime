
using api.Application.Users.Queries.GetUsers;
using api.Application.Users.Queries.GetUserById;
using api.Application.Users.Queries.GetCurrentUser;
using api.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace api.API.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsers(
        [FromServices] GetUsersHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new GetUsersQuery(),
            ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new GetUserByIdQuery(id),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser(
        [FromServices] GetCurrentUserHandler handler,
        CancellationToken ct)
    {
        // ⚠️ Temporal hasta JWT
        var currentUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var result = await handler.Handle(
            new GetCurrentUserQuery(currentUserId),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }
}
