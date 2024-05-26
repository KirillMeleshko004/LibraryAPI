using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Library.Infrastructure
{
     /// <summary>
    /// Class helps injecting infrastructure dependencies
    /// </summary>
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            return services;
        }
    }
}