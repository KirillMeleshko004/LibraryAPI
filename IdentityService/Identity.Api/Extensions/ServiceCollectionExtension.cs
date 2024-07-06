using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Identity.Api.Controllers;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
      //Register and configure logger for application
      public static IServiceCollection ConfigureLogger(this IServiceCollection services)
      {
         return services.AddSerilog(configuration =>
         {
            configuration.WriteTo.Console();
         });
      }

      //Adding Cors policies to application
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

      //Configure all data store and retrieve options, 
      //like database context and repositories
      public static IServiceCollection ConfigureData(this IServiceCollection services,
         IConfiguration configuration)
      {
         return services.AddDbContext<RepositoryContext>(options =>
         {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));

            options.UseOpenIddict();
         });
      }

      //Configuring API controllers and related services
      public static IServiceCollection ConfigurePresentationControllers(this IServiceCollection services)
      {
         services.AddControllers(options =>
         {
            //return http 406 Not Acceptable when Accept header contains
            //unsupported format
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
         }).AddApplicationPart(typeof(AuthorizationController).Assembly);

         //Supressing default 400 bad request response on invalid model
         services.Configure<ApiBehaviorOptions>(options =>
         {
            options.SuppressModelStateInvalidFilter = true;
         });

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

               options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate()
                   .DisableAccessTokenEncryption();

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

      public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
      {
         services.AddSwaggerGen(options =>
         {
            options.SwaggerDoc("v0",
               new OpenApiInfo
               {
                  Title = "Identity API v0",
                  Version = "v0"
               });

            var xmlFileName = $"{typeof(Identity.Controllers.AssemblyReference)
               .Assembly.GetName().Name}.xml";
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

         return services;
      }

   }
}