using Library.Infrastructure.Data;
using Library.UseCases.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
   /// <summary>
   /// Class helps injecting infrastructure dependencies
   /// </summary>
   public static class InfrastructureServiceExtension
   {
      public static IServiceCollection AddInfrastructureServices(
         this IServiceCollection services)
      {
         services.ConfigureRepositorties();

         return services;
      }

      private static void ConfigureRepositorties(
         this IServiceCollection services)
      {
         services.AddTransient<IRepositoryManager, RepositoryManager>();
      }
   }
}