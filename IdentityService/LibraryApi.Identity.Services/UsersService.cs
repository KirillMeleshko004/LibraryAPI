using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
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

      public async Task<TokenDto> GetTokenAsync(string email)
      {
         var user = await _userManager.FindByEmailAsync(email)
            ?? throw new Exception("User not found");

         //Receive list of claims for user
         var claims = await GetClaimsAsync(user);

         var credentials = GetSigningCredentials();

         var tokenDescriptor = GetTokenDescriptor(claims, credentials);

         var accessToken = new JsonWebTokenHandler().CreateToken(tokenDescriptor);

         return new TokenDto {AccessToken = accessToken};
      }

      public async Task<(IdentityResult result, User? user)> CreateUserAsync(
         UserForCreationDto userDto)
      {
         
         var user = _mapper.Map<User>(userDto);
         user.UserName = user.Email;

         var res = await _userManager.CreateAsync(user, userDto.Password);

         return (res, user);
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
            claims.Add(new(ClaimTypes.Role, role));
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
         var a = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtOptions.Expires));

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

      #endregion

   }
}