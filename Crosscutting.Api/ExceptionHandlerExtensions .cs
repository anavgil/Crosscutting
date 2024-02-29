using Croscutting.Common.Configurations.Exception;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crosscutting.Api;
public class ExceptionHandlerExtensions
{
    #region Members

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerExtensions> _logger;
    private readonly ExceptionSettingsBinder _options;

    #endregion

    #region Constructor

    public ExceptionHandlerExtensions(RequestDelegate next, ILogger<ExceptionHandlerExtensions> logger, IOptions<ExceptionSettingsBinder> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    #endregion

    #region Public Methods

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    #endregion

    #region Private Methods

    private Task HandleException(HttpContext context, Exception ex)
    {
        // Handle exception using UseExceptionHandler
        var exceptionHandlerFeature = new ExceptionHandlerFeature()
        {
            Error = ex,
            Path = string.Empty//_options.RedirectPath
        };
        context.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);

        return _next(context);
    }

    #endregion
}
