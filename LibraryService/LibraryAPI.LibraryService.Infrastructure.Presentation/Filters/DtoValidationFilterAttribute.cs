using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryAPI.LibraryService.Infrastructure.Presentation.Filters
{
   /// <summary>
   /// Validate dto action parameter. Action must have parameter with "dto" in type name
   /// </summary>
   public class DtoValidationFilter : IActionFilter
   {
      public void OnActionExecuted(ActionExecutedContext context)
      {
      }

      public void OnActionExecuting(ActionExecutingContext context)
      {
         var action = context.RouteData.Values["action"];
         var controller = context.RouteData.Values["controller"];

         //Searching action method argument which type contains Dto. E.g. BookDto
         //Actiong arguments kvp contains object like this:
         //key: book
         //value: { BookDto { bookDtoFields } }
         var param = context.ActionArguments
            .SingleOrDefault(p => p.Value!.ToString()!
               .Contains("dto", StringComparison.InvariantCultureIgnoreCase))
               .Value;

         //If param is null, it means no object was passed for Dto argument
         if (param == null)
         {
            context.Result = new BadRequestObjectResult("Dto object is null. " +
               $"Controller: {controller}, action: {action}.");
            return;
         }

         if (!context.ModelState.IsValid)
         {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
         }
      }
   }
}