namespace Identity.Shared.Results
{
   /// <summary>
   /// Class represents result of operation
   /// </summary>
   public class Result<T>
   {
      public ResultStatus Status { get; set; }
      public IEnumerable<string>? Errors { get; set; }
      public IEnumerable<ValidationError>? ValidationErrors { get; set; }
      public T? Value { get; set; }

      protected Result(){ }

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
            Errors = [ errorInfo ]
         }; 
      }

      public static Result<T> InvalidData(ValidationError errorInfo)
      {
         return new Result<T>(ResultStatus.InvalidData)
         {
            ValidationErrors = [ errorInfo ]
         }; 
      }

      public static Result<T> InvalidData(IEnumerable<ValidationError> errorsInfo)
      {
         return new Result<T>(ResultStatus.InvalidData)
         {
            ValidationErrors = errorsInfo
         }; 
      }

      public static Result<T> Unauthorized()
      {
         return new Result<T>(ResultStatus.Unauthorized); 
      }

      public static Result<T> Unauthorized(string errorInfo)
      {
         return new Result<T>(ResultStatus.Unauthorized)
         {
            Errors = [ errorInfo ]
         }; 
      }

      public static Result<T> Error(string errorInfo)
      {
         return new Result<T>(ResultStatus.Error)
         {
            Errors = [ errorInfo ]
         }; 
      }

   }
}