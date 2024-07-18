using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Controllers
{
    public static class ControllersServiceExtension
    {
        public static IServiceCollection ConfigurePresentation(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ViewModels.BookForCreationViewModel).Assembly);

            return services;
        }
    }
}