using Library.Infrastructure.Data;
using Library.Infrastructure.Images;
using Library.UseCases.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
   /// <summary>
   /// Class helps injecting infrastructure dependencies
   /// </summary>
   public static class InfrastructureServiceExtension
   {
      public static IServiceCollection ConfigureInfrastructureServices(
         this IServiceCollection services, IConfiguration configuration)
      {
         services.ConfigureDB(configuration)
            .ConfigureRepositorties();
         services.AddScoped<IImageService, ImageService>();

         return services;
      }

      private static IServiceCollection ConfigureRepositorties(
         this IServiceCollection services)
      {
         return services.AddTransient<IRepositoryManager, RepositoryManager>();
      }

      private static IServiceCollection ConfigureDB(this IServiceCollection services,
         IConfiguration configuration)
      {
         services.AddDbContext<RepositoryContext>(options =>
         {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDb"));
         });

         return services;
      }
   }
}