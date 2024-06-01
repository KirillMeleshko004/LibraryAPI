using AutoMapper;
using Identity.Domain.Entities;
using Identity.Shared.Results;
using Identity.UseCases.Common.Messages;
using Identity.UseCases.Common.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Handler for AuthorizeUserHandler
   /// </summary>
   public class AuthorizeUserHandler : IRequestHandler<AuthorizeUserCommand, Result<Token>>
   {

      private readonly UserManager<User> _userManager;
      private readonly TokenIssuer _tokenIssuer;
      private readonly ILogger<AuthorizeUserHandler> _logger;
      public AuthorizeUserHandler(UserManager<User> userManager, TokenIssuer tokenIssuer,
         ILogger<AuthorizeUserHandler> logger)
      {
         _userManager = userManager;
         _tokenIssuer = tokenIssuer;
         _logger = logger;
      }
      public async Task<Result<Token>> Handle(AuthorizeUserCommand request, 
         CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByEmailAsync(request.UserDto.Email);

         var res = user != null &&
            await _userManager.CheckPasswordAsync(user, request.UserDto.Password);

         if(!res) 
         {
            _logger.LogInformation(
               LoggingMessages.AuthorizationFailedLog
            );

            return Result<Token>.Unauthorized(ResponseMessages.AuthorizationFailed);
         }

         return  Result<Token>.Success(
            await _tokenIssuer.CreateTokenAsync(user!, extendLifetime: true));
      }
   }
}