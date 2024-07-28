using Identity.Api.Common.Configuration;
using Identity.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace Identity.Api.HostedServices
{
    /// <summary>
    /// Creates ensure all cliens from configuration created.
    /// In production should be run locally, not during normal app pipeline
    /// </summary>
    public class ClientsConfiguration : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly ClientsOptions _clients;
        public ClientsConfiguration(IOptions<ClientsOptions> clients, IServiceProvider services)
        {
            _services = services;
            _clients = clients.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

            var appManager =
                scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            foreach (var client in _clients.ClientsArr)
            {
                if (await appManager.FindByClientIdAsync(client.ClientId, cancellationToken) is null)
                {
                    OpenIddictApplicationDescriptor appDescriptor = new()
                    {
                        ClientId = client.ClientId,
                        ClientSecret = client.ClientSecret,
                        DisplayName = client.DisplayName,
                    };

                    if (client.RedirectUris != null)
                    {
                        appDescriptor.RedirectUris.UnionWith(client.RedirectUris);
                    }
                    if (client.Permissions != null)
                    {
                        appDescriptor.Permissions.UnionWith(client.Permissions);
                    }

                    await appManager.CreateAsync(appDescriptor, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}