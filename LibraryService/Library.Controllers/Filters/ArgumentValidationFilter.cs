using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.Controllers.Filters
{
   /// <summary>
   /// Perfoms validation on arguments.
   /// Needed to pass argumet names to validate names in ctor.
   /// </summary>
   public class ArgumentValidationFilterAttribute : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         if (!context.ModelState.IsValid)
         {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
         }

      }
   }
}