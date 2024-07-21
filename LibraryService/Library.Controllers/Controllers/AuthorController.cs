using Library.Controllers.Filters;
using Library.UseCases.Authors.Commands;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Authors.Queries;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Controllers
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
      /// <param name="cancellationToken"></param>
      /// <returns>List of authors. Could be empty</returns>
      ///<response code="200">Returns list of authors</response>
      ///<response code="400">If some request parameters has invalid values</response>
      ///<response code="404">If none of authors match request</response>
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetAuthors([FromQuery] AuthorParameters parameters,
         CancellationToken cancellationToken)
      {
         var result = await _sender.Send(new ListAuthorsQuery(parameters), cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Retrieve author based on id
      /// </summary>
      /// <param name="id">id of required author</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Author</returns>
      ///<response code="200">Returns author</response>
      ///<response code="404">If author with id not found</response>
      [HttpGet("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetAuthorById(Guid id, CancellationToken cancellationToken)
      {
         var result = await _sender.Send(new GetAuthorByIdQuery(id), cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Creates author
      /// </summary>
      /// <param name="authorDto">represents author to create</param>
      /// <param name="cancellationToken"></param>
      /// <returns>A newly created author</returns>
      ///<response code="201">Returns created author</response>
      ///<response code="400">If authorDto is null</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="422">If authorDto contains invalid fields</response>
      [HttpPost]
      [Authorize(policy: "admin")]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> CreateAuthor([FromBody] AuthorForCreationDto authorDto,
         CancellationToken cancellationToken)
      {

         var result = await _sender.Send(new CreateAuthorCommand(authorDto), cancellationToken);

         return CreatedAtAction(nameof(GetAuthorById),
            new { id = result.Id }, result);
      }

      /// <summary>
      /// Updates existing author
      /// </summary>
      /// <param name="id">id of author to update</param>
      /// <param name="authorDto">represents new author's values</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Nothing</returns>
      ///<response code="204">If author updated successfully</response>
      ///<response code="400">If authorDto is null</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="404">If author with id not found</response>
      ///<response code="422">If authorDto contains invalid fields</response>
      [HttpPut("{id:guid}")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> UpdateAuthor(Guid id,
         [FromBody] AuthorForUpdateDto authorDto, CancellationToken cancellationToken)
      {
         await _sender.Send(new UpdateAuthorCommand(id, authorDto), cancellationToken);

         return NoContent();
      }

      /// <summary>
      /// Deletes a specific author
      /// </summary>
      /// <param name="id">id of required author</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Nothing</returns>
      ///<response code="204">If author deleted or not exist</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      [HttpDelete("{id:guid}")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      public async Task<IActionResult> DeleteAuthor(Guid id, CancellationToken cancellationToken)
      {
         await _sender.Send(new DeleteAuthorCommand(id), cancellationToken);

         return NoContent();
      }

      /// <summary>
      /// Retrieve list of books based on author id and request parameters
      /// </summary>
      /// <param name="id">Id of autrhor</param>
      /// <param name="parameters">Parameters that describes what books should be retrieved</param>
      /// <param name="cancellationToken"></param>
      /// <returns>List of books by author. Could be empty</returns>
      ///<response code="200">Returns list of books</response>
      ///<response code="400">If some request parameters has invalid values</response>
      ///<response code="404">If author with id not found OR he has no books</response>
      [HttpGet("{id:guid}/books")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBooksForAuthor(Guid id,
         [FromQuery] BookParameters parameters, CancellationToken cancellationToken)
      {
         var result =
            await _sender.Send(new ListBooksByAuthorQuery(parameters, id), cancellationToken);

         return Ok(result);
      }
   }
}