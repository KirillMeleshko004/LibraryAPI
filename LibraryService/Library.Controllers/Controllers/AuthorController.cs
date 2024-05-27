using Library.Shared.Results;
using Library.UseCases.Authors.Queries;
using Library.UseCases.Common.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Api.Controllers
{
   [ApiController]
   [Route("api/authors")]
   public class AuthorController : ControllerBase
   {
      private readonly ISender _sender;
      private readonly ILogger<AuthorController> _logger;

      public AuthorController(ISender sender, ILogger<AuthorController> logger)
      {
         _sender = sender;
         _logger = logger;
      }

      /// <summary>
      /// Retrieve list of authors base on request parameters
      /// </summary>
      /// <param name="parameters">Parameters that describes what authors should be retrieved</param>
      /// <returns>List of authors. Could be empty</returns>
      ///<response code="200">Returns list of authors</response>
      ///<response code="400">If some request parameters has invalid values</response>
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> GetAuthors([FromQuery] AuthorParameters parameters)
      {   
         var result = await _sender.Send(new ListAuthorsQuery(parameters));

         if(result.Status == ResultStatus.Ok) return Ok(result.Value);

         else
         {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
         }
         
      }

      /// <summary>
      /// Retrieve author based on id
      /// </summary>
      /// <param name="id">id of required author</param>
      /// <returns>Author</returns>
      ///<response code="200">Returns author</response>
      ///<response code="404">If author with id not found</response>
      [HttpGet("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetAuthorById(Guid id)
      {
         var result = await _sender.Send(new GetAuthorByIdQuery(id));

         if (result.Status == ResultStatus.NotFound)
         {
            return NotFound(result.Errors);
         }

         return Ok(result.Value);
      }

      // /// <summary>
      // /// Creates author
      // /// </summary>
      // /// <param name="authorDto">represents author to create</param>
      // /// <returns>A newly created author</returns>
      // ///<response code="201">Returns created author</response>
      // ///<response code="400">If authorDto is null</response>
      // ///<response code="401">If authorize header missing or contains invalid token</response>
      // ///<response code="422">If authorDto contains invalid fields</response>
      // [HttpPost]
      // [Authorize]
      // [ServiceFilter(typeof(DtoValidationFilter))]
      // [ProducesResponseType(StatusCodes.Status201Created)]
      // [ProducesResponseType(StatusCodes.Status400BadRequest)]
      // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      // [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      // public async Task<IActionResult> CreateAuthor([FromBody] AuthorForCreationDto authorDto)
      // {

      //    var res = await _services.Authors.CreateAuthorAsync(authorDto);

      //    if (res.Status == OpStatus.Fail)
      //    {
      //       return StatusCode((int)res.Error!.ErrorType,
      //          new { message = res.Error.Description });
      //    }

      //    return CreatedAtAction(nameof(GetAuthorById),
      //       new { id = res.Value!.Id },
      //       res.Value);

      // }

      // /// <summary>
      // /// Updates existing author
      // /// </summary>
      // /// <param name="id">id of author to update</param>
      // /// <param name="authorDto">represents new author's values</param>
      // /// <returns>Nothing</returns>
      // ///<response code="204">If author updated successfully</response>
      // ///<response code="400">If authorDto is null</response>
      // ///<response code="401">If authorize header missing or contains invalid token</response>
      // ///<response code="404">If author with id not found</response>
      // ///<response code="422">If authorDto contains invalid fields</response>
      // [HttpPut("{id:guid}")]
      // [Authorize]
      // [ServiceFilter(typeof(DtoValidationFilter))]
      // [ProducesResponseType(StatusCodes.Status204NoContent)]
      // [ProducesResponseType(StatusCodes.Status400BadRequest)]
      // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      // [ProducesResponseType(StatusCodes.Status404NotFound)]
      // [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      // public async Task<IActionResult> UpdateAuthor(Guid id,
      //    [FromBody] AuthorForUpdateDto authorDto)
      // {
      //    var res = await _services.Authors.UpdateAuthorAsync(id, authorDto);

      //    if (res.Status == OpStatus.Fail)
      //    {
      //       return StatusCode((int)res.Error!.ErrorType,
      //          new { message = res.Error.Description });
      //    }

      //    return NoContent();
      // }

      // /// <summary>
      // /// Deletes a specific author
      // /// </summary>
      // /// <param name="id">id of required author</param>
      // /// <returns>Nothing</returns>
      // ///<response code="204">If author deleted or not exist</response>
      // ///<response code="401">If authorize header missing or contains invalid token</response>
      // [HttpDelete("{id:guid}")]
      // [Authorize]
      // [ProducesResponseType(StatusCodes.Status204NoContent)]
      // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      // public async Task<IActionResult> DeleteAuthor(Guid id)
      // {
      //    await _services.Authors.DeleteAuthorAsync(id);

      //    return NoContent();
      // }


   }
}