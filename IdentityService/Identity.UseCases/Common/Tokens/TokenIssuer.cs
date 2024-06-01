using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Domain.Entities;
using Identity.Shared.Results;
using Identity.UseCases.Common.Configuration;
using Identity.UseCases.Common.Exceptions;
using Identity.UseCases.Common.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Identity.UseCases.Common.Tokens
{
   public class TokenIssuer
   {
      private readonly UserManager<User> _userManager;
      private readonly JwtOptions _options;
      private readonly  ILogger<TokenIssuer> _logger;

      public TokenIssuer(UserManager<User> userManager, IOptions<JwtOptions> options,
         ILogger<TokenIssuer> logger)
      {
         _userManager = userManager;
         _options = options.Value;
         _logger = logger;
      }

      internal async Task<Token> CreateTokenAsync(User user, bool extendLifetime)
      {
         //Receive list of claims for user
         var claims = await GetClaimsAsync(user);
         var credentials = GetSigningCredentials();

         var tokenDescriptor = GetTokenDescriptor(claims, credentials);

         var accessToken = new JsonWebTokenHandler().CreateToken(tokenDescriptor);
         var refreshToken = GetRefreshToken();

         user.RefreshToken = refreshToken;

         if(extendLifetime)
         {
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
         }

         await _userManager.UpdateAsync(user);

         return new Token { AccessToken = accessToken, RefreshToken = refreshToken };
      }

      //Issue new TokenDto object. If expired token invalid returns null
      internal async Task<Token?> RefreshTokenAsync(Token expiredToken)
      {
         var jwt = await GetTokenFromExpiredAsync(expiredToken.AccessToken);
         
         if(jwt == null) 
         {
            return null;
         }

         var email = jwt.GetClaim(JwtRegisteredClaimNames.Email).Value.ToString();

         if(!await IsRefreshTokenValid(expiredToken.RefreshToken, email)) return null;

         var user = await _userManager.FindByEmailAsync(email);

         if(user == null) 
         {
            return null;
         }

         var token = await CreateTokenAsync(user, false);

         return token;
      }

      #region Private methods

      //Returns list of claims associated with user
      private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
      {
         var claims = new List<Claim>
         {
            new (JwtRegisteredClaimNames.Email, user.Email!)
         };

         var roles = await _userManager.GetRolesAsync(user);

         foreach(var role in roles)
         {
            //Not Claims.Role since Ocelot has problem with parsing its name
            claims.Add(new("Roles", role));
         }
         
         return claims;
      }

      //Returns token descriptor to issue token based on it
      private SecurityTokenDescriptor GetTokenDescriptor(IEnumerable<Claim> claims, 
         SigningCredentials credentials)
      {
         var descriptor = new SecurityTokenDescriptor
         {
            Issuer = _options.ValidIssuer,
            Audience = _options.ValidAudience,
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_options.Expires)),
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims)
         };

         return descriptor;
      }

      //Returns signing credentials to sing JWT token
      private static SigningCredentials GetSigningCredentials()
      {
         var secretKey = Environment.GetEnvironmentVariable(JwtOptions.SECRET_ENV) ??
            throw new SecretKeyNotSetException();

         //Convert string key to array of bytes
         var bytes = Encoding.UTF8.GetBytes(secretKey);
         //Create sekurityKey object, which is used to generate token
         var key = new SymmetricSecurityKey(bytes);

         return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      }

      //Create random 64 bytes string
      private static string GetRefreshToken()
      {
         var randBytes = new byte[64];
         var gen = RandomNumberGenerator.Create();

         gen.GetBytes(randBytes);

         return Convert.ToBase64String(randBytes);
      }

      //Retrieve JsonWebToken from expired token
      //return null if token invalid
      private async Task<JsonWebToken?> GetTokenFromExpiredAsync(string expiredToken)
      {
         var secretKey = Environment.GetEnvironmentVariable(JwtOptions.SECRET_ENV) ??
            throw new SecretKeyNotSetException();

         var handler = new JsonWebTokenHandler();
         
         var res = await handler.ValidateTokenAsync(expiredToken, 
            new TokenValidationParameters
            {
               ValidateAudience = true,
               ValidateIssuer = true,
               ValidateLifetime = false,
               ValidateIssuerSigningKey = true,

               ValidIssuer = _options.ValidIssuer,
               ValidAudience = _options.ValidAudience,
               IssuerSigningKey = 
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)) 
            });

         if(!res.IsValid) 
         {
            _logger.LogWarning(LoggingMessages.ExpiredValidationFailedLog, 
               res.Exception.Message);
            return null;
         }

         return handler.ReadJsonWebToken(expiredToken);
      }

      private async Task<bool> IsRefreshTokenValid(string refreshToken, string email)
      {
         var token = (await _userManager.FindByEmailAsync(email))?.RefreshToken;

         if(token == null)
         {
            _logger.LogInformation(LoggingMessages.RefreshTokenExpiredLog);
         }

         return refreshToken.Equals(token);
      }

      #endregion
      
   }
}