namespace LibraryAPI.LibraryService.Domain.Interfaces.Loggers
{
    public interface ILibraryLogger
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarn(string message);
        void LogError(string message);
    }
}