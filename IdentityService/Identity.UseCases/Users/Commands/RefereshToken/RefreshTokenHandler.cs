using Identity.Shared.Results;
using Identity.UseCases.Common.Messages;
using Identity.UseCases.Common.Tokens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Handler for RefreshTokenCommand
   /// </summary>
   public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<Token>>
   {
      private readonly TokenIssuer _tokenIssuer;
      private readonly ILogger<RefreshTokenHandler> _logger;
      public RefreshTokenHandler(TokenIssuer tokenIssuer,
         ILogger<RefreshTokenHandler> logger)
      {
         _tokenIssuer = tokenIssuer;
         _logger = logger;
      }

      public async Task<Result<Token>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
      {
         var token = await _tokenIssuer.RefreshTokenAsync(request.ExpiredToken);

         if(token == null)
         {
            _logger.LogInformation(
               LoggingMessages.TokenRefreshFailedLog
            );
            return Result<Token>.Unauthorized(ResponseMessages.TokenRefreshFailed);
         }

         return Result<Token>.Success(token);
      }
   }
}