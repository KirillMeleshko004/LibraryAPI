using FluentValidation;
using Library.UseCases.Common.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Library.UseCases
{
    public static class UseCasesServiceCollectionExtension
    {
        public static IServiceCollection ConfigureApplicationServices(
            this IServiceCollection services)
        {
            //pass assembly with mapping profiles
            services.AddAutoMapper(typeof(BookProfile).Assembly)
                .AddValidatorsFromAssembly(typeof(UseCasesServiceCollectionExtension).Assembly);

            services.AddMediatR(config =>
                config.RegisterServicesFromAssemblies(typeof(BookProfile).Assembly));

            return services;
        }
    }
}