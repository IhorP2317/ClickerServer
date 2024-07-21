using Clicker.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clicker.API.Filters;

public class GlobalExceptionHandlingFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionHandlingFilter> _logger;

    public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        _logger.LogError("{msg}, {stc}", exception.Message, exception.StackTrace);

        var problemDetails = exception switch
        {
            ApiException apiException => new ProblemDetails
            {
                Status = apiException.StatusCode,
                Title = "API exception occurred",
                Detail = apiException.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Server error",
                Detail = exception.Message
            }
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;

        await Task.CompletedTask;
    }
}