using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LibraryApi.Identity.Domain.Core.ConfigModels;
using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Domain.Interfaces.Loggers;
using LibraryApi.Identity.Domain.Interfaces.Services;
using LibraryApi.Identity.Infrastructure.Data.Contexts;
using LibraryApi.Identity.Infrastructure.Logger;
using LibraryApi.Identity.Services;
using LibraryAPI.Identity.Infrastructure.Presentation;
using LibraryAPI.Identity.Infrastructure.Presentation.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;

namespace LibraryApi.Identity.Web.Extensions
{
   /// <summary>
   /// Class contains exstensions to simplify app configuration and dependency injection
   /// </summary>
   public static class ServiceCollectionExtension
   {
      //Register and configure logger for application
      public static void ConfigureLogger(this IServiceCollection services)
      {
         LogManager.Setup(options =>
         {
            options.LoadConfigurationFromXml(string.Concat(
               Directory.GetCurrentDirectory(), "nlog.config"
            ));
         });

         services.TryAddSingleton<IIdentityLogger, IdentityLogger>();
      }

      //Adding Cors policies to application
      public static void ConfigureCors(this IServiceCollection services)
      {
         services.AddCors(options =>
         {
            options.AddPolicy("Default", builder =>
            {
               builder.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin();
            });
         });
      }

      //Configure all data store and retrieve options, 
      //like database context and repositories
      public static void ConfigureData(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.AddDbContext<RepositoryContext>(options =>
         {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));
         });
      }

      //Configure ServiceManager (UoW) and other consumed services, like mapper
      public static void ConfigureServices(this IServiceCollection services)
      {
         services.AddAutoMapper(typeof(Program).Assembly);
         services.TryAddScoped<IServiceManager, ServiceManager>();
      }

      //Configuring API controllers and related services
      public static void ConfigurePresentationControllers(this IServiceCollection services)
      {
         services.AddControllers(options =>
         {
            //return http 406 Not Acceptable when Accept header contains
            //unsupported format
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
         }).AddApplicationPart(typeof(AssemblyReference).Assembly);

         //Supressing default 400 bad request response on invalid model
         services.Configure<ApiBehaviorOptions>(options =>
         {
            options.SuppressModelStateInvalidFilter = true;
         });

         services.AddScoped<DtoValidationFilter>();
      }

      //Configure ASP.NET Data protection keys
      public static void ConfigureDataProtection(this IServiceCollection services)
      {
         //if OS is windows leave default configuration with DPAPI
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;


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

         services.AddAuthorization();
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

      //Configure microsoft Identity to work with stored users
      public static void ConfigureIdentity(this IServiceCollection services)
      {
         services.AddIdentity<User, Role>(options =>
         {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 10;

            options.User.RequireUniqueEmail = true;
         })
         .AddEntityFrameworkStores<RepositoryContext>();

      }

      //Register TOption instances in DI container (Microsft options pattern)
      public static void ConfigureOptions(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
      }

      public static void ConfigureSwagger(this IServiceCollection services)
      {
         services.AddSwaggerGen(options =>
         {
            options.SwaggerDoc("v0",
               new OpenApiInfo
               {
                  Title = "Identity API v0",
                  Version = "v0"
               });

            var xmlFileName = $"{typeof(AssemblyReference).Assembly.GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlFilePath);

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
               new OpenApiSecurityScheme
               {
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.ApiKey,
                  Scheme = JwtBearerDefaults.AuthenticationScheme,
                  Description = "JWT Authorization header using the Bearer scheme."
               });

            //Add an authorization header to each endpoint when the request is sent. 
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
                        Id = JwtBearerDefaults.AuthenticationScheme
                     },
                     Name = JwtBearerDefaults.AuthenticationScheme
                  },
                  //the value of the dictionary is a required list of scope names 
                  //for the execution only if the security scheme is oauth2 or openIdConnect
                  new List<string>()
               }
            });
         });
      }

   }
}