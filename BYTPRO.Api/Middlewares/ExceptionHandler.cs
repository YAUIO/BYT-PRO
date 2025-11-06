using Microsoft.AspNetCore.Diagnostics;

namespace BYTPRO.Api.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // We return true when the exception is handled, and false when not
        // We should set appropriate return code and body in httpContext.Response via httpContext.Response.StatusCode and httpContext.Response.WriteAsJsonAsync(new (error = "error"))
        return false;
    }
}