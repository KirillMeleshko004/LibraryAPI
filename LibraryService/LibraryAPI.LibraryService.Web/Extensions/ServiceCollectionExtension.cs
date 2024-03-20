using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Infrastructure.Logging;
using NLog;

namespace LibraryAPI.LibraryService.Web
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureLogging(this IServiceCollection services)
        {
            LogManager.Setup().LoadConfigurationFromFile(
                string.Concat(Directory.GetCurrentDirectory(), "nlog.config")
            );

            services.AddSingleton<ILibraryLogger, LibraryLogger>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
        }
    }
}