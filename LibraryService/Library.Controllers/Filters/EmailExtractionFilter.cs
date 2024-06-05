using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.Controllers.Filters
{
   /// <summary>
   /// Filter retrieve email from claims add it to context items.
   /// </summary>
   /// <param name="argName">Key to put email with in items.</param>
   public class EmailExtractionFilterAttribute(string argName) : ActionFilterAttribute
   {
      private readonly string _argName = argName;

      public override void OnActionExecuting(ActionExecutingContext context)
      {
         var email = context.HttpContext.User.Claims
            .First(c => c.Type == ClaimTypes.Email).Value;

         if (string.IsNullOrWhiteSpace(email) ||
            !new EmailAddressAttribute().IsValid(email))
         {
            context.Result = new ForbidResult("Email claim missing or incorrect.");
            return;
         }

         context.HttpContext.Items.Add(_argName, email);

      }
   }
}