using Crosscutting.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crosscutting.Api.Middlewares;

public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private const string validationExceptionTitle = "One or more validation errors occurred.";
    private const string validationExceptionType = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    private const string standarExceptionTitle = "One error occurred.";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = GetStatuscodeFromException(exception),
            Detail = exception.Message
        };

        if (exception is CustomValidationException fluentException)
        {
            problemDetails.Title = validationExceptionTitle;
            problemDetails.Type = validationExceptionType;
            List<string> validationErrors = [];
            foreach (var error in fluentException.Errors)
            {
                validationErrors.Add(error.Error);
            }
            problemDetails.Extensions.Add("errors", validationErrors);
        }
        else
        {
            problemDetails.Title = standarExceptionTitle;
            problemDetails.Type = exception.GetType().Name;

            if (exception.InnerException is not null)
                problemDetails.Extensions = new Dictionary<string, object>()
            {
                { "INNER-Message",exception.InnerException.Message },
                { "INNER-Type",exception.InnerException.GetType().Name}
            };
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

    }

    private static int GetStatuscodeFromException(Exception exception) => exception switch
    {
        BadHttpRequestException => StatusCodes.Status400BadRequest,
        CustomValidationException => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };
}
