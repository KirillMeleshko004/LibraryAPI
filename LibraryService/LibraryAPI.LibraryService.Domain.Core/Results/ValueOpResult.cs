namespace LibraryAPI.LibraryService.Domain.Core.Results
{
   public class ValueOpResult<T> : OpResult
   {
      
      public T? Value { get; set; }

   }
}