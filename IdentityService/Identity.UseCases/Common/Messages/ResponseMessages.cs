namespace Identity.UseCases.Common.Messages
{
   public static class ResponseMessages
   {
      public static string AuthorizationFailed => "Authorization failed. Wrong user name or password.";
      public static string TokenRefreshFailed => 
         "Token refresh failed";
   }
}