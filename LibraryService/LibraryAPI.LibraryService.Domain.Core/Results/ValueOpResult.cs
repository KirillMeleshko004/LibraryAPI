namespace LibraryAPI.LibraryService.Domain.Core.Results
{
   /// <summary>
   /// Class represents result of operation which return something
   /// </summary>
   public class ValueOpResult<T> : OpResult
   {

      public T? Value { get; set; }

   }
}