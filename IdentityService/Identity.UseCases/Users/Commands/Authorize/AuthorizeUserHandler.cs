using AutoMapper;
using Identity.Domain.Entities;
using Identity.UseCases.Common.Messages;
using Identity.UseCases.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Handler for AuthorizeUserHandler
   /// </summary>
   public class AuthorizeUserHandler : IRequestHandler<AuthorizeUserCommand>
   {

      private readonly UserManager<User> _userManager;
      private readonly ILogger<AuthorizeUserHandler> _logger;
      public AuthorizeUserHandler(UserManager<User> userManager,
         ILogger<AuthorizeUserHandler> logger)
      {
         _userManager = userManager;
         _logger = logger;
      }

      public async Task Handle(AuthorizeUserCommand request,
         CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByNameAsync(request.UserDto.UserName);

         var res = user != null &&
            await _userManager.CheckPasswordAsync(user, request.UserDto.Password);

         if (!res)
         {
            _logger.LogInformation(
               LoggingMessages.AuthorizationFailedLog
            );

            throw new UnauthorizedException("Invalid username/password pair.");
         }
      }
   }
}