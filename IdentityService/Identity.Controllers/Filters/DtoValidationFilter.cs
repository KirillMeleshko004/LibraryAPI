using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Controllers.Filters
{
   /// <summary>
   /// Filter to validate action dtos. Needed to pass dto to validate names in ctor
   /// </summary>
   public class DtoValidationFilterAttribute(params string[] names) : ActionFilterAttribute
   {
      private readonly string[] _argumentNames = names;

      public override void OnActionExecuting(ActionExecutingContext context)
      {
         var action = context.RouteData.Values["action"];
         var controller = context.RouteData.Values["controller"];

         foreach(var arg in _argumentNames)
         {
            if(!context.ActionArguments.ContainsKey(arg))
            {
               context.Result = new BadRequestObjectResult($"{arg} object is null. " +
                  $"Controller: {controller}, action: {action}.");
               return;
            }
         }

         if (!context.ModelState.IsValid)
         {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
         }
      }
   }
}