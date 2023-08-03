using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Crosscutting.Base;
public class ExceptionHandlerExtensions
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerExtensions(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Execute next middleware
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private Task HandleException(HttpContext context, Exception ex)
    {
        // Handle exception using UseExceptionHandler
        var exceptionHandlerFeature = new ExceptionHandlerFeature()
        {
            Error = ex,
        };
        context.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);

        // Invoque middleware UseExceptionHandler to handle exception
        context.Request.Path = "/Error"; // Path can be replaced

        return _next(context);
    }
}
