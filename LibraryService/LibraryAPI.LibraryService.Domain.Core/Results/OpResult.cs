namespace LibraryAPI.LibraryService.Domain.Core.Results
{
   /// <summary>
   /// Class represents result of operation
   /// </summary>
   public class OpResult
   {
      public OpStatus Status { get; set; }
      public OpError? Error { get; set; }

      public static OpResult SuccessResult()
      {
         return new OpResult
         {
            Status = OpStatus.Success
         };
      }

      //Provides method to create operation result
      #region Result Creators

      public static OpResult FailResult(string errorDescription, ErrorType errorType)
      {
         return new OpResult
         {
            Status = OpStatus.Fail,
            Error = new OpError
            {
               ErrorType = errorType,
               Description = errorDescription
            }
         };
      }

      public static ValueOpResult<T> SuccessValueResult<T>(T value)
      {
         return new ValueOpResult<T>
         {
            Status = OpStatus.Success,
            Value = value
         };
      }

      public static ValueOpResult<T> FailValueResult<T>(string errorDescription,
         ErrorType errorType)
      {
         return new ValueOpResult<T>
         {
            Status = OpStatus.Fail,
            Error = new OpError
            {
               ErrorType = errorType,
               Description = errorDescription
            }
         };
      }

      public static ValueOpResult<T> FailValueResult<T>(OpResult failRes)
      {
         return new ValueOpResult<T>
         {
            Status = OpStatus.Fail,
            Error = failRes.Error
         };
      }

      #endregion

   }
}