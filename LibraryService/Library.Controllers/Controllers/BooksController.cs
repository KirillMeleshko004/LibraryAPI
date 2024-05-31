using Library.Controllers.Filters;
using Library.Shared.Results;
using Library.UseCases.Books.Commands;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Api.Controllers
{
   [ApiController]
   [Route("api/books")]
   public class BooksController : ControllerBase
   {
      private readonly ISender _sender;
      private readonly ILogger<BooksController> _logger;

      public BooksController(ISender sender, ILogger<BooksController> logger)
      {
         _sender = sender;
         _logger = logger;
      }

      /// <summary>
      /// Retrieve book based on id
      /// </summary>
      /// <param name="id">id of required book</param>
      /// <returns>Book</returns>
      ///<response code="200">Returns book</response>
      ///<response code="404">If book with id not found</response>
      [HttpGet("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBookById(Guid id)
      {
         var result = await _sender.Send(new GetBookByIdQuery(id));

         if (result.Status == ResultStatus.NotFound)
         {
            return NotFound(result.Errors);
         }

         return Ok(result.Value);
      }

      /// <summary>
      /// Retrieve book based on ISBN
      /// </summary>
      /// <param name="isbn">isbn of required book</param>
      /// <returns>Book</returns>
      ///<response code="200">Returns book</response>
      ///<response code="404">If book with isbn not found</response>
      [HttpGet("isbn/{isbn}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBookByISBN(string isbn)
      {
         var result = await _sender.Send(new GetBookByISBNQuery(isbn));

         if (result.Status == ResultStatus.NotFound)
         {
            return NotFound(result.Errors);
         }

         return Ok(result.Value);
      }

      /// <summary>
      /// Retrieve list of books base on request parameters
      /// </summary>
      /// <param name="parameters">Parameters that describes what books should be retrieved</param>
      /// <returns>List of books. Could be empty</returns>
      ///<response code="200">Returns list of books</response>
      ///<response code="400">If some request parameters has invalid values</response>
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<IActionResult> GetBooks([FromQuery] BookParameters parameters)
      {
         var result = await _sender.Send(new ListBooksQuery(parameters));

         return Ok(result.Value);
      }

      /// <summary>
      /// Creates book
      /// </summary>
      /// <param name="bookForCreation">represents book to create</param>
      /// <returns>A newly book book</returns>
      ///<response code="201">Returns created book</response>
      ///<response code="400">If bookDto is null</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="404">If author with id specified in bookDto doesn't exist</response>
      ///<response code="422">If bookDto contains invalid fields</response>
      [HttpPost]
      [Authorize]
      [DtoValidationFilter(names: "bookForCreation")]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> CreateBook(
         [FromBody] BookForCreationDto bookForCreation)
      {
         var result = await _sender.Send(
            new CreateBookCommand(bookForCreation, bookForCreation.AuthorId));

         if (result.Status == ResultStatus.NotFound)
         {
            return NotFound(result.Errors);
         }

         return CreatedAtAction(nameof(GetBookById),
            new { id = result.Value!.Id }, result.Value);
      }

      /// <summary>
      /// Updates existing book
      /// </summary>
      /// <param name="id">id of book to update</param>
      /// <param name="bookForUpdate">represents new book's values</param>
      /// <returns>Nothing</returns>
      ///<response code="204">If book updated successfully</response>
      ///<response code="400">If bookDto is null</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="404">If book with id not found or author with id specified in bookDto doesn't exist</response>
      ///<response code="422">If bookDto contains invalid fields</response>
      [HttpPut("{id:guid}")]
      [Authorize]
      [DtoValidationFilter(names: "bookForUpdate")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> UpdateBook(Guid id,
         [FromBody] BookForUpdateDto bookForUpdate)
      {
         var result = await _sender.Send(
            new UpdateBookCommand(id, bookForUpdate));

         if (result.Status == ResultStatus.NotFound)
         {
            return NotFound(result.Errors);
         }

         return NoContent();
      }

      /// <summary>
      ///  Deletes a specific book
      /// </summary>
      /// <param name="id">id of required book</param>
      /// <returns>Nothing</returns>
      ///<response code="204">If book deleted or not exist</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      [HttpDelete("{id:guid}")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      public async Task<IActionResult> DeleteBookById(Guid id)
      {
         var result = await _sender.Send(new DeleteBookCommand(id));

         return NoContent();
      }
   }
}