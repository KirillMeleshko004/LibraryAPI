using AutoMapper;
using Library.Infrastructure.Data;
using Library.Infrastructure.Images;
using Library.UseCases.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
   /// <summary>
   /// Class helps injecting infrastructure dependencies
   /// </summary>
   public static class InfrastructureServiceExtension
   {
      public static IServiceCollection ConfigureInfrastructureServices(
         this IServiceCollection services)
      {
         services.ConfigureRepositorties();
         services.AddScoped<IImageService, ImageService>();

         return services;
      }

      private static void ConfigureRepositorties(
         this IServiceCollection services)
      {
         services.AddTransient<IRepositoryManager, RepositoryManager>();
      }
   }
}