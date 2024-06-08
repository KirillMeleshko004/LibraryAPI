using Identity.Api.Common;

namespace Identity.Api.Middlewares
{
   public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
      RequestDelegate next)
   {
      private readonly ILogger<ExceptionMiddleware> _logger = logger;
      private readonly RequestDelegate _next = next;


      public async Task InvokeAsync(HttpContext context)
      {
         try
         {
            await _next.Invoke(context);
         }
         catch (Exception ex)
         {
            _logger.LogError("Exception happened during request execution: {ex}", ex);
            await HandleExceptionAsync(context, ex);
         }
      }

      private async Task HandleExceptionAsync(HttpContext context, Exception exception)
      {
         context.Response.ContentType = "application/json";
         context.Response.StatusCode = StatusCodes.Status500InternalServerError;

         await context.Response.WriteAsync(new ErrorDetails
         {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "Internal Server Error. Something went wrong."
         }.ToString());
      }
   }
}