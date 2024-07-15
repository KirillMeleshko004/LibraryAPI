using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Library.Api.Common;
using Library.Api.Utility;
using Library.Infrastructure.Data;
using Library.Infrastructure.Images;
using Library.UseCases.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Library.Api.Extensions
{
   /// <summary>
   /// Class contains exstensions to simplify app configuration and dependency injection
   /// </summary>
   public static class ServiceCollectionExtension
   {
      //Register and configure logger for application
      public static void ConfigureLogging(this IServiceCollection services)
      {
         services.AddSerilog(configuration =>
         {
            configuration.WriteTo.Console();
         });
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
      public static void ConfigureApplicationServices(this IServiceCollection services,
         IConfiguration configuration)
      {
         //pass assembly with mapping profiles
         services.AddAutoMapper(typeof(UseCases.AssemblyReference).Assembly);

         services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(typeof(UseCases.AssemblyReference).Assembly));

         services.Configure<ImageOptions>(configuration.GetSection(ImageOptions.SectionName));
      }

      public static void ConfigureEF(this IServiceCollection services,
         IConfiguration configuration)
      {
         //Register RepositoryContext as scoped service
         services.AddDbContext<RepositoryContext>(options =>
         {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));
         });
      }

      //Configuring API controllers and related services
      public static void ConfigureControllers(this IServiceCollection services)
      {
         services.AddControllers(options =>
         {
            //return http 406 Not Acceptable when Accept header contains unsupported format
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
         })
         .AddJsonOptions(options =>
         {
            options.JsonSerializerOptions.Converters.Insert(0, new DateOnlyJsonConverter());
         })
         .AddApplicationPart(typeof(Library.Controllers.AssemblyReference).Assembly);

         //Supressing default 400 bad request response on invalid model
         services.Configure<ApiBehaviorOptions>(options =>
         {
            options.SuppressModelStateInvalidFilter = true;
         });

         // services.AddScoped<DtoValidationFilter>();
      }

      public static void ConfigureSwagger(this IServiceCollection services)
      {
         services.AddSwaggerGen(options =>
         {
            options.SwaggerDoc("v0",
               new OpenApiInfo
               {
                  Title = "Library API",
                  Version = "v0",
                  Description = "An ASP.NET Core Web API for managing managing books and authors.",
               });

            var xmlFileName = $"{typeof(Library.Controllers.AssemblyReference).Assembly.GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlFilePath);

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


      public static IServiceCollection ConfigureOpenIdDict(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.AddOpenIddict()
            .AddValidation(options =>
            {
               //TODO
               //Move certs to different folder
               options.AddEncryptionCertificate(
                  new X509Certificate2(File.ReadAllBytes("../../CertCreator/Certs/encryption-certificate.pfx")));
               options.AddSigningCertificate(
                  new X509Certificate2(File.ReadAllBytes("../../CertCreator/Certs/signing-certificate.pfx")));
               options.SetIssuer("https://localhost:7213");
               options.UseSystemNetHttp();
               options.UseAspNetCore();
            });

         services
            .AddAuthentication(options =>
            {
               options.DefaultAuthenticateScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
               options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

         return services;
      }
   }
}