using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Crosscutting.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;

        _logger.LogError(exception,
            "Error Message: {}, Time  of occurrence {time}", exceptionMessage, DateTime.UtcNow);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "An error occurred",
            Detail = exception.Message,
            Type = exception.GetType().Name
        };

        if (exception.InnerException is not null)
            problemDetails.Extensions = new Dictionary<string, object>()
            {
                { "INNER-Message",exception.InnerException.Message },
                { "INNER-Type",exception.InnerException.GetType().Name}
            };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;

    }
}
