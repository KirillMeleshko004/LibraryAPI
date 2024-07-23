using AutoMapper;
using Library.Controllers.ViewModels;
using Library.Controllers.Filters;
using Library.UseCases.Books.Commands;
using Library.UseCases.Books.DTOs;
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
   [Route("api/books")]
   public class BooksController : ControllerBase
   {
      private readonly ISender _sender;
      private readonly IMapper _mapper;
      private readonly ILogger<BooksController> _logger;

      public BooksController(ISender sender, IMapper mapper, ILogger<BooksController> logger)
      {
         _sender = sender;
         _mapper = mapper;
         _logger = logger;
      }

      /// <summary>
      /// Retrieve book based on id
      /// </summary>
      /// <param name="id">id of required book</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Book</returns>
      ///<response code="200">Returns book</response>
      ///<response code="404">If book with id not found</response>
      [HttpGet("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBookById(Guid id, CancellationToken cancellationToken)
      {
         var result = await _sender.Send(new GetBookByIdQuery(id), cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Retrieve book based on ISBN
      /// </summary>
      /// <param name="isbn">isbn of required book</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Book</returns>
      ///<response code="200">Returns book</response>
      ///<response code="404">If book with isbn not found</response>
      [HttpGet("isbn/{isbn}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBookByISBN(string isbn, CancellationToken cancellationToken)
      {
         var result = await _sender.Send(new GetBookByISBNQuery(isbn), cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Retrieve list of books base on request parameters
      /// </summary>
      /// <param name="parameters">Parameters that describes what books should be retrieved</param>
      /// <param name="cancellationToken"></param>
      /// <returns>List of books</returns>
      ///<response code="200">Returns list of books</response>
      ///<response code="400">If some request parameters has invalid values</response>
      ///<response code="404">If none of books match request</response>
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBooks([FromQuery] BookParameters parameters,
         CancellationToken cancellationToken)
      {
         var result = await _sender.Send(new ListBooksQuery(parameters), cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Creates book
      /// </summary>
      /// <param name="bookVm">represents book to create</param>
      /// <param name="cancellationToken"></param>
      /// <returns>A newly book book</returns>
      ///<response code="201">Returns created book</response>
      ///<response code="400">If bookDto is null</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="422">If bookDto contains invalid fields</response>
      [HttpPost]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> CreateBook(
         [FromForm] BookForCreationViewModel bookVm, CancellationToken cancellationToken)
      {
         var bookForCreation = _mapper.Map<BookForCreationDto>(bookVm);

         var result = await _sender.Send(
            new CreateBookCommand(bookForCreation), cancellationToken);

         return CreatedAtAction(nameof(GetBookById),
            new { id = result.Id }, result);
      }

      /// <summary>
      /// Updates existing book
      /// </summary>
      /// <param name="id">id of book to update</param>
      /// <param name="bookVm">represents new book's values</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Nothing</returns>
      ///<response code="204">If book updated successfully</response>
      ///<response code="400">If bookDto is null</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="404">If book with id not found</response>
      ///<response code="422">If bookDto contains invalid fields</response>
      [HttpPut("{id:guid}")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> UpdateBook(Guid id,
         [FromBody] BookForUpdateViewModel bookVm, CancellationToken cancellationToken)
      {
         var bookForUpdate = _mapper.Map<BookForUpdateDto>(bookVm);

         await _sender.Send(new UpdateBookCommand(id, bookForUpdate), cancellationToken);

         return NoContent();
      }

      /// <summary>
      ///  Deletes a specific book
      /// </summary>
      /// <param name="id">id of required book</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Nothing</returns>
      ///<response code="204">If book deleted or not exist</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      [HttpDelete("{id:guid}")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      public async Task<IActionResult> DeleteBookById(Guid id, CancellationToken cancellationToken)
      {
         await _sender.Send(new DeleteBookCommand(id), cancellationToken);

         return NoContent();
      }

      /// <summary>
      /// Borrows specific book
      /// </summary>
      /// <param name="id">Id of book to borrow</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Nothing</returns>
      ///<response code="204">If book borrowed successfully</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="403">If email claim is incorrect or empty</response>
      ///<response code="404">If book to borrow not found</response>
      [HttpPost("{id:guid}/borrow")]
      [Authorize]
      [EmailExtractionFilter("email")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> BorrowBook(Guid id, CancellationToken cancellationToken)
      {
         string email = (HttpContext.Items["email"] as string)!;

         await _sender.Send(
            new BorrowBookCommand(email, id), cancellationToken);

         return NoContent();
      }

      /// <summary>
      /// Returns specific book
      /// </summary>
      /// <param name="id">Id of book to return</param>
      /// <param name="cancellationToken"></param>
      /// <returns>Nothing</returns>
      ///<response code="204">If book borrowed successfully</response>
      ///<response code="401">If authorize header missing or contains invalid token</response>
      ///<response code="403">If email claim is incorrect or empty</response>
      ///<response code="404">If book to return not found OR if reader not found</response>
      ///<response code="422">If reader attemts to return book he don't have</response>
      [HttpPost("{id:guid}/return")]
      [Authorize]
      [EmailExtractionFilter("email")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
      public async Task<IActionResult> ReturnBook(Guid id, CancellationToken cancellationToken)
      {
         string email = (HttpContext.Items["email"] as string)!;

         await _sender.Send(
            new ReturnBookCommand(email, id), cancellationToken);

         return NoContent();
      }

      /// <summary>
      /// Retrieve list of books that reader borrowed
      /// </summary>
      /// <param name="cancellationToken"></param>
      /// <returns>List of books</returns>
      ///<response code="200">Returns list of books</response>
      ///<response code="400">If some request parameters has invalid values</response>
      ///<response code="404">If reader hasn't borrowed any books</response>
      [HttpGet("myBooks")]
      [Authorize]
      [EmailExtractionFilter("email")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetBooksForReader(CancellationToken cancellationToken)
      {
         string email = (HttpContext.Items["email"] as string)!;

         var result = await _sender.Send(new ListBooksForReaderQuery(email), cancellationToken);

         return Ok(result);
      }

   }
}