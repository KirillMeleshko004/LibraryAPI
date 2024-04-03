using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Shared.Dtos;
using Microsoft.AspNetCore.Identity;

namespace LibraryApi.Identity.Domain.Interfaces.Services
{
   public interface IUsersService
   {
      
      Task<bool> IsUserValidAsync(UserForAuthorizationDto userDto);

      Task<TokenDto> GetTokenAsync(string email);
      
      Task<TokenDto?> RefreshTokenAsync(TokenDto expiredToken);

      Task<(IdentityResult result, User? user)> CreateUserAsync(UserForCreationDto userDto);
   }
}