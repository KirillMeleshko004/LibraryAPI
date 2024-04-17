using AutoMapper;
using LibraryApi.Identity.Domain.Core.ConfigModels;
using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Domain.Interfaces.Loggers;
using LibraryApi.Identity.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LibraryApi.Identity.Services
{
   /// <summary>
   /// Class represents single unit of work for services.
   /// ServiceManager instance created once for each request and ensures
   /// that all services use same instance of UserManage 
   /// </summary>
   public class ServiceManager : IServiceManager
   {
      //Make services Lazy to save resources if service not used during request
      private readonly Lazy<IUsersService> _users;

      public IUsersService Users 
      { 
         get 
         {
            return _users.Value;
         }
      }

      public ServiceManager(UserManager<User> userManager, IIdentityLogger logger,
         IMapper mapper, IOptions<JwtOptions> options)
      {
         _users = new Lazy<IUsersService>(
            () => new UsersService(userManager, logger, mapper, options.Value));
      }
   }
}