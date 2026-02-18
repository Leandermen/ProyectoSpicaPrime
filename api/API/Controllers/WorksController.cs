using Microsoft.AspNetCore.Authorization;
using api.Application.Works.Commands.StartWork;
using api.Application.Works.Commands.SuspendWork;
using api.Application.Works.Commands.ResumeWork;
using api.Application.Works.Commands.CompleteWork;
using api.Application.Works.Commands.CancelWork;
using api.Application.Works.Queries.GetWorksByUser;
using api.API.Contracts;
using api.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace api.API.Controllers;

[ApiController]
[Route("api/works")]
[Authorize]
public sealed class WorksController : ControllerBase
{
    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUser(
        Guid userId,
        [FromServices] GetWorksByUserHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new GetWorksByUserQuery(userId),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Start(
        Guid id,
        [FromServices] StartWorkHandler handler,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var command = new StartWorkCommand(id);

        var result = await handler.Handle(
            userId,
            command,
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpPost("{id:guid}/suspend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Suspend(
        Guid id,
        [FromBody] SuspendWorkRequest request,
        [FromServices] SuspendWorkHandler handler,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var result = await handler.Handle(
            userId,
            new SuspendWorkCommand(id, request.Reason),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpPost("{id:guid}/resume")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Resume(
        Guid id,
        [FromServices] ResumeWorkHandler handler,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var result = await handler.Handle(
            userId,
            new ResumeWorkCommand(id),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromServices] CompleteWorkHandler handler,
        CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);
        
        var result = await handler.Handle(
            userId,
            new CompleteWorkCommand(id),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelWorkRequest request,
        [FromServices] CancelWorkHandler handler,
        CancellationToken ct)        
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var result = await handler.Handle(
            userId,
            new CancelWorkCommand(id, request.Reason),
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }
}
