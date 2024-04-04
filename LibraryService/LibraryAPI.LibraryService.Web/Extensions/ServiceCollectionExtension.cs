using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;
using LibraryAPI.LibraryService.Infrastructure.Data.Repos;
using LibraryAPI.LibraryService.Infrastructure.Logging;
using LibraryAPI.LibraryService.Infrastructure.Presentation;
using LibraryAPI.LibraryService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;

namespace LibraryAPI.LibraryService.Web.Extensions
{
    /// <summary>
    /// Class contains exstensions to simplify app configuration and dependency injection
    /// </summary>
    public static class ServiceCollectionExtension
    {
        //Register and configure logger for application
        public static void ConfigureLogging(this IServiceCollection services)
        {
            LogManager.Setup().LoadConfigurationFromFile(
                string.Concat(Directory.GetCurrentDirectory(), "nlog.config")
            );

            services.AddSingleton<ILibraryLogger, LibraryLogger>();
        }

        //Adding Cors policies to application
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

        //Configure application services and their dependencies
        public static void ConfigureServices(this IServiceCollection services)
        {
            //pass assembly with mapping profiles
            services.AddAutoMapper(typeof(Program).Assembly);

            //RepositoryManager is scoped since it represents single unit of work
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        //Configure all data store and retrieve options, 
        //like database context and repositories
        public static void ConfigureData(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Register RepositoryContext as scoped service
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));
            });

            //RepositoryManager is scoped since it represents single unit of work
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        //Configuring controllers, specify assebly
        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //return http 406 Not Acceptable when Accept header contains
                //unsupported format
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }).AddApplicationPart(typeof(AssemblyReference).Assembly);
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v0",
                    new OpenApiInfo
                    {
                        Title = "Library API",
                        Version = "v0"
                    });
            });
        }
    }
}