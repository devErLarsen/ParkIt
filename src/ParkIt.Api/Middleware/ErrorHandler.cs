using System.Net;
using ParkIt.Core.Exceptions;

namespace ParkIt.Api.Middleware;

public class ErrorHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    private static async Task HandleException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        var (statusCode, message) = exception switch
        {
            ParkItException => new Error(StatusCodes.Status400BadRequest, string.Join(": ", exception.GetType().Name, exception.Message)) ,
            _ => new Error(StatusCodes.Status500InternalServerError, "Something went wrong in the server") 
        };
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(message);
    }

    private record Error(int StatusCode, string Message);
}