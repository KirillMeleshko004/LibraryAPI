using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using NLog;

namespace LibraryAPI.LibraryService.Infrastructure.Logging
{
    /// <summary>
    /// Wrapper class for NLog logger
    /// </summary>
    public class LibraryLogger : ILibraryLogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }
        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
        public void LogError(string message)
        {
            _logger.Error(message);
        }
    }
}