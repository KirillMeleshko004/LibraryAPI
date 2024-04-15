namespace LibraryAPI.LibraryService.Domain.Core.Results
{
   public class OpError
   {
      public string Description { get; set; } = null!;

      public ErrorType ErrorType { get; set; }

   }
}