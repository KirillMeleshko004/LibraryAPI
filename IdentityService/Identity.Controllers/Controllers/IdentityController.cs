using System.Security.Claims;
using Identity.Controllers.Filters;
using Identity.Domain.Entities;
using Identity.UseCases.Users.Commands;
using Identity.UseCases.Users.Dtos;
using Identity.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Identity.Controllers
{
    [ApiController]
    [Route("api")]
    public class IdentityController : ControllerBase
    {
        private readonly ISender _sender;

        public IdentityController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Authorize user based on provided data
        /// </summary>
        /// <returns>Tokens base on requested scopes</returns>
        ///<response code="200">Returns tokens</response>
        ///<response code="400">If userDto is null</response>
        ///<response code="401">If email or password invalid</response>
        ///<response code="422">If userDto contains invalid fields</response>
        [HttpPost("connect/token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Exchange([FromForm] UserForAuthorizationDto userDto,
            CancellationToken cancellationToken)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
               throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                await _sender.Send(new AuthorizeUserCommand(userDto), cancellationToken);

                // Get claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = await _sender.Send(
                    new GetUserClaimsQuery(userDto.UserName, request.GetScopes()),
                    cancellationToken);

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(new ClaimsPrincipal(identity),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            }
            throw new InvalidOperationException("The specified grant type is not supported.");
        }


        /// <summary>
        /// Refresh expired access token
        /// </summary>
        /// <returns>New tokens</returns>
        ///<response code="200">Returns new tokens</response>
        ///<response code="401">If refresh token invalid</response>
        [HttpPost("refresh")]
        [DtoValidationFilter("expiredToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
               throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var authResult = await HttpContext
                .AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
            {
                return Unauthorized(authResult.Failure!.Message);
            }

            if (request.IsRefreshTokenGrantType())
            {

                var username = authResult.Principal.GetClaim(OpenIddictConstants.Claims.Name);
                await _sender.Send(new ValidateNameCommand(username), cancellationToken);

                var identity = await _sender.Send(
                    new GetUserClaimsQuery(username!, request.GetScopes()),
                    cancellationToken);

                return SignIn(new ClaimsPrincipal(identity),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            throw new InvalidOperationException("Invalid grant type.");
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="userDto">represents user to create</param>
        /// <returns>Nothing</returns>
        ///<response code="201">If success. Returns nothing</response>
        ///<response code="400">If userDto is null</response>
        ///<response code="422">If userDto contains invalid fields</response>
        [HttpPost("register")]
        [DtoValidationFilter("userDto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] UserForCreationDto userDto)
        {
            await _sender.Send(new CreateUserCommand(userDto));

            return Created();
        }

    }
}