namespace Library.Shared.Results
{
   /// <summary>
   /// Class represents result of operation
   /// </summary>
   public class Result<T>
   {
      public ResultStatus Status { get; set; }
      public IEnumerable<string>? Errors { get; set; }
      public T? Value { get; set; }

      protected Result() { }

      protected Result(ResultStatus status)
      {
         Status = status;
      }

      protected Result(T value)
      {
         Value = value;
      }

      public static Result<T> Success(T value)
      {
         return new Result<T>(value);
      }

      public static Result<T> NotFound()
      {
         return new Result<T>(ResultStatus.NotFound);
      }

      public static Result<T> NotFound(string errorInfo)
      {
         return new Result<T>(ResultStatus.NotFound)
         {
            Errors = [errorInfo]
         };
      }

      public static Result<T> InvalidData(string errorInfo)
      {
         return new Result<T>(ResultStatus.InvalidData)
         {
            Errors = [errorInfo]
         };
      }

      public static Result<T> Error(string errorInfo)
      {
         return new Result<T>(ResultStatus.Error)
         {
            Errors = [errorInfo]
         };
      }

   }
}