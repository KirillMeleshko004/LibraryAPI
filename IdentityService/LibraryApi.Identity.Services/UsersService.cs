using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using LibraryApi.Identity.Domain.Core.ConfigModels;
using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Domain.Interfaces.Loggers;
using LibraryApi.Identity.Domain.Interfaces.Services;
using LibraryApi.Identity.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApi.Identity.Services
{
   /// <summary>
   /// Service represent business-logic for users
   /// </summary>
    public class UsersService : IUsersService
    {
      
      private readonly UserManager<User> _userManager;
      private readonly IIdentityLogger _logger;
      private readonly IMapper _mapper;
      private readonly JwtOptions _jwtOptions;
      
      public UsersService(UserManager<User> userManager, IIdentityLogger logger,
         IMapper mapper, JwtOptions options)
      {
         _userManager = userManager;
         _logger = logger;
         _mapper = mapper;
         _jwtOptions = options;
      }

      //Validate if email and password are correct
      public async Task<bool> IsUserValidAsync(UserForAuthorizationDto userDto)
      {
         var user = await _userManager.FindByEmailAsync(userDto.Email);

         var res = user != null &&
            await _userManager.CheckPasswordAsync(user, userDto.Password);

         if(!res) 
         {
            _logger.LogWarn(
                  $"{nameof(IsUserValidAsync)}: Authentication failed. Wrong user name or password."
               );

            return false;
         }

         return res;
      }

      //Issue new TokenDto with refresh tokend time prolongation
      public async Task<TokenDto> GetTokenAsync(string email)
      {
         var user = await _userManager.FindByEmailAsync(email)
            ?? throw new Exception("User not found");

         var token = await CreateTokenAsync(user, true);

         await _userManager.UpdateAsync(user);

         return token;
      }

      //Creates new user
      public async Task<(IdentityResult result, User? user)> CreateUserAsync(
         UserForCreationDto userDto)
      {
         
         var user = _mapper.Map<User>(userDto);
         user.UserName = user.Email;

         var res = await _userManager.CreateAsync(user, userDto.Password);

         if(res.Succeeded)
         {
            await _userManager.AddToRolesAsync(user, userDto.UserRoles);
         }

         return (res, user);
      }

      //Issue new TokenDto object. If expired token invalid returns null
      public async Task<TokenDto?> RefreshTokenAsync(TokenDto expiredToken)
      {
         var jwt = await GetValidTokenAsync(expiredToken.AccessToken);
         
         if(jwt == null) return null;

         var email = jwt.GetClaim(JwtRegisteredClaimNames.Email).Value.ToString();

         if(!await IsRefreshTokenValid(expiredToken.RefreshToken, email)) return null;

         var user = await _userManager.FindByEmailAsync(email)
            ?? throw new Exception($"User with email: {email} not found");

         var token = await CreateTokenAsync(user, false);

         await _userManager.UpdateAsync(user);

         return token;
      }


      #region Private methods

      //Creates tokenDto objcect, with new access and refresh tokens
      //new refresh token wrote to user (need to call Update on DB to save)
      private async Task<TokenDto> CreateTokenAsync(User user, bool extendLifetime)
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

         return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
      }

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
      
      //Returns signing credentials to sing JWT token
      private SigningCredentials GetSigningCredentials()
      {
         var secretKey = Environment.GetEnvironmentVariable("LIBRARY_SECRET") ??
            throw new Exception("Secret key didn't setted");

         //Convert string key to array of bytes
         var bytes = Encoding.UTF8.GetBytes(secretKey);
         //Create sekurityKey object, which is used to generate token
         var key = new SymmetricSecurityKey(bytes);

         return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      }

      //Returns token descriptor to issue token based on it
      private SecurityTokenDescriptor GetTokenDescriptor(IEnumerable<Claim> claims, 
         SigningCredentials credentials)
      {
         var descriptor = new SecurityTokenDescriptor
         {
            Issuer = _jwtOptions.ValidIssuer,
            Audience = _jwtOptions.ValidAudience,
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtOptions.Expires)),
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims)
         };

         return descriptor;
      }

      //Create random 64 bytes string
      private string GetRefreshToken()
      {
         var randBytes = new byte[64];
         var gen = RandomNumberGenerator.Create();

         gen.GetBytes(randBytes);

         return Convert.ToBase64String(randBytes);
      }

      //Retrieve JsonWebToken from expired token
      //return null if token invalid
      private async Task<JsonWebToken?> GetValidTokenAsync(string expiredToken)
      {
         var secretKey = Environment.GetEnvironmentVariable("LIBRARY_SECRET") ??
            throw new Exception("Secret key didn't setted");

         var handler = new JsonWebTokenHandler();
         
         var res = await handler.ValidateTokenAsync(expiredToken, 
            new TokenValidationParameters
            {
               ValidateAudience = true,
               ValidateIssuer = true,
               ValidateLifetime = false,
               ValidateIssuerSigningKey = true,

               ValidIssuer = _jwtOptions.ValidIssuer,
               ValidAudience = _jwtOptions.ValidAudience,
               IssuerSigningKey = 
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)) 
            });

         if(!res.IsValid) 
         {
            _logger.LogWarn(
                  $"{nameof(GetValidTokenAsync)}: expired token validation failed. Reason: {res.Exception.Message}."
               );

            return null;
         }

         return handler.ReadJsonWebToken(expiredToken);
      }

      private async Task<bool> IsRefreshTokenValid(string refreshToken, string email)
      {
         var token = (await _userManager.FindByEmailAsync(email))?.RefreshToken;

         return refreshToken.Equals(token);
      }

      #endregion

   }
}