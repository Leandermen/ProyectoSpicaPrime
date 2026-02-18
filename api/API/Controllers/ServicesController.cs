using Microsoft.AspNetCore.Authorization;
using api.Application.Services.Queries.GetAvailableServices;
using api.Application.Services.Queries.GetServiceById;
using api.Application.Services.Commands.RequestService;
using api.Application.Common.Interfaces;
using api.API.Contracts.Services;
using api.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace api.API.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public sealed class ServicesController : ControllerBase
{
    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable(
        [FromServices] GetAvailableServicesHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new GetAvailableServicesQuery(), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();


    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetServiceByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new GetServiceByIdQuery(id), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    [HttpPost("{id:guid}/request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RequestService(
        Guid id,
        [FromBody] RequestServiceRequest request,
        [FromServices] RequestServiceHandler handler,
        [FromServices] ICurrentUser currentUser,
        CancellationToken ct)
    {
        var clientId = currentUser.UserId;

        var command = new RequestServiceCommand(
            id,
            request.ClientNotes);

        var result = await handler.Handle(
            clientId,
            command,
            ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }
}

