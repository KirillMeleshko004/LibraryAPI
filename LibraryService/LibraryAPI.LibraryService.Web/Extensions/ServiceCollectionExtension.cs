using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;
using LibraryAPI.LibraryService.Infrastructure.Data.Repos;
using LibraryAPI.LibraryService.Infrastructure.Logging;
using LibraryAPI.LibraryService.Services;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace LibraryAPI.LibraryService.Web.Extensions
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

        public static void ConfigureServices(this IServiceCollection services)
        {
            //pass assembly with mapping profiles
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddSingleton<IServiceManager, ServiceManager>();
        }

        public static void ConfigureData(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));
            });

            services.AddSingleton<IRepositoryManager, RepositoryManager>();
        }
    }
}