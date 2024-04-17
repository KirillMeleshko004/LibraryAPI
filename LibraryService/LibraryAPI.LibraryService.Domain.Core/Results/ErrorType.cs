namespace LibraryAPI.LibraryService.Domain.Core.Results
{
   /// <summary>
   /// Represents all error types for operation with corresponding http code
   /// </summary>
   public enum ErrorType
   {
      BadRequest = 400,
      NotFound = 404,
      Internal = 500
   }
}