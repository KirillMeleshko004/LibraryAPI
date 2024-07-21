using FluentValidation;
using Library.Controllers.Common.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Library.Controllers
{
    public static class PresentationServiceExtension
    {
        public static IServiceCollection ConfigurePresentation(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ViewModels.BookForCreationViewModel).Assembly)
                .AddValidatorsFromAssembly(typeof(PresentationServiceExtension).Assembly)
                .AddFluentValidationAutoValidation(options =>
                {
                    options.EnableBodyBindingSourceAutomaticValidation = true;
                    options.EnableQueryBindingSourceAutomaticValidation = true;
                    options.EnableFormBindingSourceAutomaticValidation = true;
                });

            services.ConfigureControllers();

            return services;
        }

        //Configuring API controllers and related services
        private static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //return http 406 Not Acceptable when Accept header contains unsupported format
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Insert(0, new DateOnlyJsonConverter());
            })
            .AddApplicationPart(typeof(BooksController).Assembly);

            //Supressing default 400 bad request response on invalid model
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}