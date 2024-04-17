namespace LibraryAPI.LibraryService.Domain.Core.Results
{
   /// <summary>
   /// Represents error that occured during operation
   /// </summary>
   public class OpError
   {
      public string Description { get; set; } = null!;

      public ErrorType ErrorType { get; set; }

   }
}