namespace Library.Shared.Results
{
   /// <summary>
   /// Class represents empty result of operation
   /// </summary>
   public class Result : Result<Result>
   {
      protected Result() : base() { }

      protected Result(ResultStatus status) : base(status) { }

      public static Result Success()
      {
         return new Result();
      }

      new public static Result NotFound()
      {
         return new Result(ResultStatus.NotFound);
      }

      new public static Result NotFound(string errorInfo)
      {
         return new Result(ResultStatus.NotFound)
         {
            Errors = [errorInfo]
         };
      }

      new public static Result InvalidData(string errorInfo)
      {
         return new Result(ResultStatus.InvalidData)
         {
            Errors = [errorInfo]
         };
      }

      new public static Result Error(string errorInfo)
      {
         return new Result(ResultStatus.Error)
         {
            Errors = [errorInfo]
         };
      }
   }
}