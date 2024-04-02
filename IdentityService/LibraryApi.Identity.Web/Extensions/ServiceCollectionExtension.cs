using System.Security.Claims;
using System.Text;
using LibraryApi.Identity.Domain.Core.ConfigModels;
using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Domain.Interfaces.Loggers;
using LibraryApi.Identity.Domain.Interfaces.Services;
using LibraryApi.Identity.Infrastructure.Data.Contexts;
using LibraryApi.Identity.Infrastructure.Logger;
using LibraryApi.Identity.Services;
using LibraryAPI.Identity.Infrastructure.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace LibraryApi.Identity.Web.Extensions
{
   public static class ServiceCollectionExtension
   {
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

      public static void ConfigurePresentationControllers(this IServiceCollection services)
      {
         services.AddControllers(options =>
         {
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
         }).AddApplicationPart(typeof(AssemblyReference).Assembly);
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
   }
}