using System.Security.Claims;
using Identity.Controllers.Filters;
using Identity.Controllers.Helpers;
using Identity.Domain.Entities;
using Identity.Shared.Results;
using Identity.UseCases.Users.Commands;
using Identity.UseCases.Users.Dtos;
using Identity.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore;
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
        private readonly PasswordFlowHelper _pfHelper;

        public IdentityController(ISender sender, UserManager<User> userManager,
           SignInManager<User> signInManager)
        {
            _sender = sender;

            _pfHelper = new(userManager, signInManager);
        }

        [HttpPost("connect/token")]
        public async Task<IActionResult> Exchange([FromForm] UserForAuthorizationDto userDto,
            CancellationToken cancellationToken)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
               throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {

                var authRes =
                    await _sender.Send(new AuthorizeUserCommand(userDto), cancellationToken);

                if (authRes.Status != ResultStatus.Ok)
                {
                    return PasswordFlowHelper.InvalidCredentialsResponse();
                }

                // Get claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = await _sender.Send(
                    new GetUserClaimsQuery(userDto.UserName, request.GetScopes()),
                    cancellationToken);

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(new ClaimsPrincipal(identity.Value!),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            }
            throw new InvalidOperationException("The specified grant type is not supported.");
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
        [DtoValidationFilter("userDto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] UserForCreationDto userDto)
        {
            var res = await _sender.Send(new CreateUserCommand(userDto));

            if (!res.Succeeded)
            {
                string error = string.Empty;
                foreach (var err in res.Errors)
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
        [DtoValidationFilter("userDto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Authorize([FromBody] UserForAuthorizationDto userDto)
        {
            var result = await _sender.Send(new AuthorizeUserCommand(userDto));

            if (result.Status != ResultStatus.Ok) return Unauthorized(result.Errors);

            return Ok(result.Value);
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
        [DtoValidationFilter("expiredToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RefreshToken()
        {
            return Ok();
        }
    }
}