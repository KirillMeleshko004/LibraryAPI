using Microsoft.Extensions.DependencyInjection;

namespace Identity.UseCases
{
    public static class UseCasesServiceCollectionExtension
    {
        public static IServiceCollection ConfigureUseCases(this IServiceCollection services)
        {
            return services.ConfigureMapper()
                .ConfigureMediatR();
        }

        private static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(UseCases.AssemblyReference).Assembly);
        }
        private static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            return services.AddMediatR(config => config.RegisterServicesFromAssemblies(
               typeof(UseCases.AssemblyReference).Assembly));
        }
    }
}