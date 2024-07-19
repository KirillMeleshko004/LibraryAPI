using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Identity.UseCases
{
    public static class UseCasesServiceCollectionExtension
    {
        public static IServiceCollection ConfigureUseCases(this IServiceCollection services)
        {
            return services.ConfigureMapper()
                .AddValidatorsFromAssembly(typeof(UseCasesServiceCollectionExtension).Assembly)
                .AddFluentValidationAutoValidation()
                .ConfigureMediatR();
        }

        private static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(UseCasesServiceCollectionExtension)
                .Assembly);
        }
        private static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            return services.AddMediatR(config => config.RegisterServicesFromAssemblies(
               typeof(UseCasesServiceCollectionExtension).Assembly));
        }
    }
}