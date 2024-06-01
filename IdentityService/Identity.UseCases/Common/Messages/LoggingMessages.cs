namespace Identity.UseCases.Common.Messages
{
   public static class LoggingMessages
   {
      public const string AuthorizationFailedLog = "Authorization failed. Wrong user name or password.";
      public const string UserCreationFailedLog = 
         "User creation failed. Reason: {reason}.";
      public const string ExpiredValidationFailedLog = 
         "Expired token validation failed. Reason: {reason}.";
      public const string RefreshTokenExpiredLog =
         "Refresh token expired.";
      public const string TokenRefreshFailedLog = "Token refresh failed.";
   }
}