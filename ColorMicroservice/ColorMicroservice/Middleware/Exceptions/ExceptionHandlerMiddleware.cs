using ColorMicroservice.Shared.Exceptions;
using ColorMicroservice.Shared.Controllers;

namespace ColorMicroservice.API.Middleware.Exceptions;

internal sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var errorResponse = exception switch
        {
            BaseException baseException => BaseResponse.Failure(baseException),
            _ => BaseResponse.Failure(exception)
        };

        context.Response.StatusCode = (int)errorResponse.Error!.StatusCode;
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
