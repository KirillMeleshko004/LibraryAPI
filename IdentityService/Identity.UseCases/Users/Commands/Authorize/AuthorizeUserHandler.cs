using AutoMapper;
using Identity.Domain.Entities;
using Identity.Shared.Results;
using Identity.UseCases.Common.Configuration;
using Identity.UseCases.Common.Helpers;
using Identity.UseCases.Tokens.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
   public class AuthorizeUserHandler : IRequestHandler<AuthorizeUserCommand, Result<TokenDto>>
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
      public async Task<Result<TokenDto>> Handle(AuthorizeUserCommand request, 
         CancellationToken cancellationToken)
      {
         var user = await _userManager.FindByEmailAsync(request.UserDto.Email);

         var res = user != null &&
            await _userManager.CheckPasswordAsync(user, request.UserDto.Password);

         if(!res) 
         {
            _logger.LogWarning(
               "Authentication failed. Wrong user name or password."
            );

            return Result<TokenDto>.Unauthorized();
         }

         return  Result<TokenDto>.Success(
            await _tokenIssuer.CreateTokenAsync(user!, extendLifetime: true));
      }
   }
}