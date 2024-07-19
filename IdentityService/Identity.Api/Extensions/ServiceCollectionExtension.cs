using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Serilog;
using Identity.UseCases.Common.Configuration;
using Identity.Api.Common.Configuration;
using Identity.Api.HostedServices;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Extensions
{
   /// <summary>
   /// Class contains exstensions to simplify app configuration and dependency injection
   /// </summary>
   public static class ServiceCollectionExtension
   {
      public static IServiceCollection ConfigureLogger(this IServiceCollection services)
      {
         return services.AddSerilog(configuration =>
         {
            configuration.WriteTo.Console();
         });
      }

      public static IServiceCollection ConfigureCors(this IServiceCollection services)
      {
         return services.AddCors(options =>
         {
            options.AddPolicy("Default", builder =>
            {
               builder.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin();
            });
         });
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

      //Configure microsoft Identity to work with stored users
      public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
      {
         services.AddIdentity<User, Role>(options =>
         {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 10;

            options.User.RequireUniqueEmail = true;

            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
         })
         .AddEntityFrameworkStores<RepositoryContext>();

         return services;
      }

      public static IServiceCollection ConfigureOpenIdDict(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.AddHostedService<ClientsConfiguration>();

         var jwt = new JwtOptions();
         configuration.GetSection(JwtOptions.Jwt).Bind(jwt);
         var scopes = new ScopesOptions();
         configuration.GetSection(ScopesOptions.Scopes).Bind(scopes);

         services.AddOpenIddict()
            .AddCore(options =>
            {
               options.UseEntityFrameworkCore()
                  .UseDbContext<RepositoryContext>();
            })
            .AddServer(options =>
            {
               options.SetTokenEndpointUris("api/connect/token", "api/refresh/token");

               options.AllowPasswordFlow()
                  .AllowRefreshTokenFlow();

               //TODO
               //Move certs to different folder
               options.AddEncryptionCertificate(
                  new X509Certificate2(File.ReadAllBytes("../../CertCreator/Certs/encryption-certificate.pfx")));
               options.AddSigningCertificate(
                  new X509Certificate2(File.ReadAllBytes("../../CertCreator/Certs/signing-certificate.pfx")));
               // .DisableAccessTokenEncryption();

               options.RegisterScopes(scopes.ValidScopes.ToArray());

               options.UseAspNetCore()
                   .EnableAuthorizationEndpointPassthrough()
                   .EnableTokenEndpointPassthrough()
                   .DisableTransportSecurityRequirement();
            })
            .AddValidation(options =>
            {
               options.Configure(conf =>
               {
                  conf.TokenValidationParameters.ValidIssuers = jwt.ValidIssuers;
               });
               options.AddAudiences(jwt.ValidAudiences.ToArray());

               options.UseAspNetCore();
               options.UseLocalServer();
            });

         services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
         });

         return services;
      }

      //Register TOption instances in DI container (Microsft options pattern)
      public static IServiceCollection ConfigureOptions(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt))
            .Configure<ClientsOptions>(configuration.GetSection(ClientsOptions.Clients))
            .Configure<ScopesOptions>(configuration.GetSection(ScopesOptions.Scopes));

         return services;
      }

   }
}