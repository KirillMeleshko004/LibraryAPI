using Library.Api.Common.Models;
using Library.UseCases.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Library.Api.Extensions
{
    internal static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app,
            ILogger<Program> logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = "application/problem + json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionFeature != null)
                    {
                        context.Response.StatusCode = exceptionFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            UnavailableException => StatusCodes.Status404NotFound,
                            UnprocessableEntityException => StatusCodes.Status422UnprocessableEntity,
                            UnauthorizedException => StatusCodes.Status401Unauthorized,
                            ForbidException => StatusCodes.Status403Forbidden,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError("Something went worng {ex}", exceptionFeature.Error.Message);

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = string.IsNullOrWhiteSpace(exceptionFeature.Error.Message) ?
                                "Something went wrong" : exceptionFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}