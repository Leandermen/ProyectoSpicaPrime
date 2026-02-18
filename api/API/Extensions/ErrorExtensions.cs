using Microsoft.AspNetCore.Mvc;
using api.Application.Common;

namespace api.API.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToProblem(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var problem = new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = statusCode
        };

        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }
}
