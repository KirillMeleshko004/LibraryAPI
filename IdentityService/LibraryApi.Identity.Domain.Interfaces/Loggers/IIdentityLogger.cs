namespace LibraryApi.Identity.Domain.Interfaces.Loggers
{
   public interface IIdentityLogger
   {

      void LogDebug(string message);
      void LogInfo(string message);
      void LogWarn(string message);
      void LogError(string message);

   }
}