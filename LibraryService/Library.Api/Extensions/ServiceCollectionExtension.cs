using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Library.Api.AuthorizationRequirements.Handlers;
using Library.Api.AuthorizationRequirements.Requirements;
using Library.Infrastructure.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Library.Api.Extensions
{
   public static class ServiceCollectionExtension
   {
      public static IServiceCollection ConfigureLogging(this IServiceCollection services)
      {
         services.AddSerilog(configuration =>
         {
            configuration.WriteTo.Console();
         });

         return services;
      }

      public static IServiceCollection ConfigureCors(this IServiceCollection services)
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

         return services;
      }

      //Register TOption instances in DI container (Microsft options pattern)
      public static IServiceCollection ConfigureOptions(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.Configure<ImageOptions>(configuration.GetSection(ImageOptions.SectionName));
         return services;
      }

      //Configure ASP.NET Data protection keys
      public static IServiceCollection ConfigureDataProtection(this IServiceCollection services)
      {
         //if OS is windows leave default configuration with DPAPI
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return services;

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

         return services;
      }

      //TODO
      public static IServiceCollection ConfigureOpenIdDict(this IServiceCollection services)
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

      public static IServiceCollection ConfigurePolicies(this IServiceCollection services)
      {
         services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

         services.AddAuthorizationBuilder()
            .AddPolicy("customer", policy =>
            {
               policy.AddRequirements(new RoleAuthorizationRequirement("customer"));
            })
            .AddPolicy("admin", policy =>
            {
               policy.AddRequirements(new RoleAuthorizationRequirement("admin"));
            });


         return services;
      }

      public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
      {
         services.AddSwaggerGen(options =>
         {
            options.SwaggerDoc("v0",
               new OpenApiInfo
               {
                  Title = "Library API",
                  Version = "v0",
                  Description = "ASP.NET Core Web API for managing managing books and authors.",
               });

            var xmlFileName = $"{typeof(Controllers.BooksController).Assembly.GetName()
               .Name}.xml";

            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlFilePath);

            options.AddSecurityDefinition("OAuth2",
               new OpenApiSecurityScheme
               {
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.OAuth2,
                  Flows = new OpenApiOAuthFlows
                  {
                     Password = new OpenApiOAuthFlow
                     {
                        TokenUrl = new Uri("https://localhost:7213/api/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                           {"api.library", "Library API"},
                           {"openid", "Identity"},
                           {"offline_access", "Refresh token"}
                        }
                     },
                  },
                  Scheme = "OAuth2",
               });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
               {
                  new OpenApiSecurityScheme
                  {
                     Reference = new OpenApiReference
                     {
                        Type = ReferenceType.SecurityScheme,
                        Id = "OAuth2"
                     },
                     Name = "OAuth2"
                  },
                  new List<string>()
               }
            });
         });

         return services;
      }

   }
}