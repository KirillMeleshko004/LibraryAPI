using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LibraryAPI.LibraryService.Domain.Core.ConfigModels;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;
using LibraryAPI.LibraryService.Infrastructure.Data.Repos;
using LibraryAPI.LibraryService.Infrastructure.Logging;
using LibraryAPI.LibraryService.Infrastructure.Presentation;
using LibraryAPI.LibraryService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        Description = "JWT Authorization header using the Bearer scheme."
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    //since OpenApiSecurityRequirement implements 
                    //Dictionary<OpenApiSecurityScheme,IList<String>>
                    //dictionary initialization syntax is used
                    {
                        new OpenApiSecurityScheme
                        {
                            //Object to allow referencing other components in the specification
                            //Reference early created security scheme 
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer"
                        },
                        //the value of the dictionary is a required list of scope names 
                        //for the execution only if the security scheme is oauth2 or openIdConnect
                        new List<string>()
                    }
                });
            });
        }

        //Configure ASP.NET Data protection keys
        public static void ConfigureDataProtection(this IServiceCollection services)
        {
            //if OS is windows leave default configuration with DPAPI
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            var keyDirName = Environment.GetEnvironmentVariable("KEY_DIR_NAME")!;
            var x509CertPath = Environment.GetEnvironmentVariable("CERT_PATH")!;
            var certPassword = Environment.GetEnvironmentVariable("CERT_PASSWORD")!;

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keyDirName))
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                {
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256,
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC
                })
                //Adding keys encryption with X509 certificate
                //Using X509 since DPAPI unavailible for linux
                .ProtectKeysWithCertificate(new X509Certificate2(x509CertPath, certPassword));
        }

        public static void ConfigureAuthentication(this IServiceCollection services,
         IConfiguration configuration)
        {  
            var jwtOptions = new JwtOptions();

            //Supply JwtOptions class with JwtOptions.SectionName values from configuration
            configuration.Bind(JwtOptions.SectionName, jwtOptions);

            //Temporary store secret key as enviromental variable
            var secretKey = Environment.GetEnvironmentVariable("LIBRARY_SECRET") ??
                throw new Exception("key not set");

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(options =>
            {
                //override default scemas
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
            options.TokenValidationParameters = 
                new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidAudience = jwtOptions.ValidAudience,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    IssuerSigningKey = key
                };
            });
        }
    }
}