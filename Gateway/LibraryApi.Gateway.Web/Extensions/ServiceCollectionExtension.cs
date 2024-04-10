using System.Text;
using LibraryApi.Gateway.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Administration;
using Ocelot.DependencyInjection;

namespace LibraryApi.Gateway.Web
{
   public static class ServiceCollectionExtension
   {

      public static void ConfigureOcelot(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.AddOcelot(configuration);
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