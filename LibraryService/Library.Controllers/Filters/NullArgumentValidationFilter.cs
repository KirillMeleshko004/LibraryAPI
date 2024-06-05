using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.Controllers.Filters
{
   /// <summary>
   /// Perfoms null-check on arguments.
   /// Needed to pass argumet names to validate names in ctor.
   /// </summary>
   public class NullArgumentValidationFilter(params string[] names) : ActionFilterAttribute
   {
      private readonly string[] _argumentNames = names;

      public override void OnActionExecuting(ActionExecutingContext context)
      {
         var action = context.RouteData.Values["action"];
         var controller = context.RouteData.Values["controller"];

         foreach (var arg in _argumentNames)
         {
            if (!context.ActionArguments.ContainsKey(arg))
            {
               context.Result = new BadRequestObjectResult($"{arg} object is null. " +
                  $"Controller: {controller}, action: {action}.");
               return;
            }
         }
      }
   }
}