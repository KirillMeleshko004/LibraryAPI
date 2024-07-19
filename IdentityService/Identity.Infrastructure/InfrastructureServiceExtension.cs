using Identity.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
   /// <summary>
   /// Class helps injecting infrastructure dependencies
   /// </summary>
   public static class InfrastructureServiceExtension
   {
      public static IServiceCollection ConfigureInfrastructureServices(
         this IServiceCollection services, IConfiguration configuration)
      {
         services.ConfigureDB(configuration);

         return services;
      }

      private static IServiceCollection ConfigureDB(this IServiceCollection services,
         IConfiguration configuration)
      {
         return services.AddDbContext<RepositoryContext>(options =>
         {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));

            options.UseOpenIddict();
         });
      }
   }
}