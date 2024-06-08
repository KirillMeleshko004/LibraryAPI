using Library.UseCases.Books.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.Controllers.Filters
{

   /// <summary>
   /// Extracts image from FormData (must be first file)
   /// and adding it to BookForManipulationDto action argument
   /// </summary>
   public class BookImageExtractionFilterAttribute : ActionFilterAttribute
   {
      public override Task OnActionExecutionAsync(ActionExecutingContext context,
         ActionExecutionDelegate next)
      {
         var image = context.HttpContext.Request.Form.Files[0];

         if (image == null
            || !image.ContentType.Contains("image"))
         {
            return base.OnActionExecutionAsync(context, next);
         }

         var bookArg = context.ActionArguments
            .First(b => typeof(BookForManipulationDto).IsAssignableFrom(b.Value!.GetType()));

         var book = bookArg.Value as BookForManipulationDto;
         book!.Image = new MemoryStream();
         image.CopyTo(book.Image);
         book.Image.Position = 0;

         book.ImageName = Path.GetFileName(image.FileName);

         context.ActionArguments[bookArg.Key] = book;

         return base.OnActionExecutionAsync(context, next);
      }
   }
}