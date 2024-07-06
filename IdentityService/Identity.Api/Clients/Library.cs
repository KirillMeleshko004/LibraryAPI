
using Identity.Infrastructure.Data.Contexts;
using OpenIddict.Abstractions;

namespace Identity.Api
{
   public class Library : IHostedService
   {
      private readonly IServiceProvider _serviceProvider;

      public Library(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      public async Task StartAsync(CancellationToken cancellationToken)
      {
         using var scope = _serviceProvider.CreateScope();

         var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
         await context.Database.EnsureCreatedAsync(cancellationToken);

         var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

         if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
         {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
               ClientId = "postman",
               ClientSecret = "postman-secret",
               DisplayName = "Postman",
               RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
               Permissions =
                  {
                     OpenIddictConstants.Permissions.Endpoints.Token,

                     OpenIddictConstants.Permissions.GrantTypes.Password,
                     OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                     OpenIddictConstants.Permissions.Prefixes.Scope + "library.user",
                     OpenIddictConstants.Permissions.Prefixes.Scope + "library.admin",
                     OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Profile,
                     OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Email,
                     OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Profile,

                     OpenIddictConstants.Permissions.ResponseTypes.Code
                  }
            }, cancellationToken);
         }
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         return Task.CompletedTask;
      }
   }
}