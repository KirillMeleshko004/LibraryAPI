using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Controllers
{
    public static class PresentationServiceExtension
    {
        public static IServiceCollection ConfigurePresentation(this IServiceCollection services)
        {
            services.ConfigureControllers();

            return services;
        }

        //Configuring API controllers and related services
        private static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //return http 406 Not Acceptable when Accept header contains
                //unsupported format
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }).AddApplicationPart(typeof(IdentityController).Assembly);

            //Supressing default 400 bad request response on invalid model
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}