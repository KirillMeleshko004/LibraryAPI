using LibraryApi.Identity.Domain.Interfaces.Services;
using LibraryApi.Identity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryAPI.Identity.Infrastructure.Presentation.Controllers
{
   [ApiController]
   [Route("api")]
   public class IdentityController : ControllerBase
   {
      private readonly IServiceManager _services;

      public IdentityController(IServiceManager services)
      {
         _services = services;
      }


      [HttpPost("reg")]
      public async Task<IActionResult> Register([FromBody]UserForCreationDto userDto)
      {
         if(!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var res = await _services.Users.CreateUserAsync(userDto);

         if(!res.result.Succeeded)
         {
            string error = string.Empty;
            foreach(var err in res.result.Errors)
            {
               error += $"{err.Code}. {err.Description}";
            }
            return BadRequest(error);
         }

         return Created();
      }

      [HttpPost("auth")]
      public async Task<IActionResult> Authorize([FromBody]UserForAuthorizationDto userDto)
      {
         if(!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         var res = await _services.Users.IsUserValidAsync(userDto);

         if(!res) return Unauthorized();

         var token = await _services.Users.GetTokenAsync(userDto.Email);

         return Ok(token);
      }

      [HttpPost("refresh")]
      public async Task<IActionResult> RefreshToken([FromBody]TokenDto expiredToken)
      {
         var token = await _services.Users.RefreshTokenAsync(expiredToken);

         if(token == null) return Unauthorized();

         return Ok(token);
      }

      [Authorize]
      [HttpGet("test")]
      public IActionResult Test()
      {
         return Ok("Authorized!");
      }
   }
}