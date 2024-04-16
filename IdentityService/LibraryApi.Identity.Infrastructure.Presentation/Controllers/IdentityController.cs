using LibraryApi.Identity.Domain.Interfaces.Services;
using LibraryApi.Identity.Shared.Dtos;
using LibraryAPI.Identity.Infrastructure.Presentation.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

      /// <summary>
      /// Creates new user
      /// </summary>
      /// <param name="userDto">represents user to create</param>
      /// <returns>Nothing</returns>
      ///<response code="201">If success. Returns nothing</response>
      ///<response code="400">If userDto is null</response>
      ///<response code="422">If userDto contains invalid fields</response>
      [HttpPost("reg")]
      [ServiceFilter(typeof(DtoValidationFilter))]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> Register([FromBody] UserForCreationDto userDto)
      {
         var res = await _services.Users.CreateUserAsync(userDto);

         if (!res.result.Succeeded)
         {
            string error = string.Empty;
            foreach (var err in res.result.Errors)
            {
               error += $"{err.Code}. {err.Description}\n";
            }
            return BadRequest(error);
         }

         return Created();
      }

      /// <summary>
      /// Authorize user base on provided data
      /// </summary>
      /// <param name="userDto">represents user to authorize</param>
      /// <returns>Access and refresh tokens</returns>
      ///<response code="200">Returns access and refresh tokens</response>
      ///<response code="400">If userDto is null</response>
      ///<response code="401">If email or password invalid</response>
      ///<response code="422">If userDto contains invalid fields</response>
      [HttpPost("auth")]
      [ServiceFilter(typeof(DtoValidationFilter))]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> Authorize([FromBody] UserForAuthorizationDto userDto)
      {
         var res = await _services.Users.IsUserValidAsync(userDto);

         if (!res) return Unauthorized();

         var token = await _services.Users.GetTokenAsync(userDto.Email);

         return Ok(token);
      }

      /// <summary>
      /// Refresh expired access token
      /// </summary>
      /// <param name="expiredToken">Expired access and active refresh token</param>
      /// <returns>New access and refresh tokens. Refresh token lifetime don't prolongate</returns>
      ///<response code="200">Returns new access and refresh tokens</response>
      ///<response code="400">If tokenDto is null</response>
      ///<response code="401">If refresh token expired or invalid</response>
      ///<response code="422">If tokenDto contains invalid field</response>
      [HttpPost("refresh")]
      [ServiceFilter(typeof(DtoValidationFilter))]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> RefreshToken([FromBody] TokenDto expiredToken)
      {
         var token = await _services.Users.RefreshTokenAsync(expiredToken);

         if (token == null) return Unauthorized();

         return Ok(token);
      }
   }
}