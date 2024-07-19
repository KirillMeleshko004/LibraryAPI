using Identity.Api.Common.Models;
using Identity.UseCases.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Identity.Api.Extensions
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
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionFeature is not null)
                    {
                        context.Response.StatusCode = exceptionFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            UnauthorizedException => StatusCodes.Status401Unauthorized,
                            UnprocessableEntityException => StatusCodes.Status422UnprocessableEntity,

                            _ => StatusCodes.Status500InternalServerError
                        };


                        logger.LogError("Exception happened during request execution: {ex}",
                            exceptionFeature.Error);

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = string.IsNullOrWhiteSpace(exceptionFeature.Error.Message) ?
                                "Something went wrong." : exceptionFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}