using LibraryAPI.LibraryService.Domain.Core.Results;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.LibraryService.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _services;
        private readonly ILibraryLogger _logger;

        public BooksController(IServiceManager serviceManager, ILibraryLogger logger)
        {
            _services = serviceManager;
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
            var book = await _services.Books.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound($"Book with id: {id} not found.");
            }

            return Ok(book);
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
            System.Console.WriteLine(HttpContext);
            var book = await _services.Books.GetBookByISBNAsync(isbn);

            if (book == null)
            {
                return NotFound($"Book with ISBN: {isbn} not found.");
            }

            return Ok(book);
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
            var books = await _services.Books.GetBooksAsync(parameters);

            return Ok(books);
        }

        /// <summary>
        /// Creates book
        /// </summary>
        /// <param name="bookForCreation">represents book to create</param>
        /// <returns>A newly book book</returns>
        ///<response code="201">Returns created book</response>
        ///<response code="400">If bookDto is null or contains invalid fields</response>
        ///<response code="401">If authorize header missing or contains invalid token</response>
        ///<response code="404">If author with id specified in bookDto doesn't exist</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBook(
            [FromBody] BookForCreationDto bookForCreation)
        {
            var res = await _services.Books.CreateBookAsync(bookForCreation);

            if (res.Status == OpStatus.Fail)
            {
                return StatusCode((int)res.Error!.ErrorType,
                    new { message = res.Error.Description });
            }

            return CreatedAtAction(nameof(GetBookById),
                new { id = res.Value!.Id },
                res.Value);
        }

        /// <summary>
        /// Updates existing book
        /// </summary>
        /// <param name="id">id of book to update</param>
        /// <param name="bookForUpdate">represents new book's values</param>
        /// <returns>Nothing</returns>
        ///<response code="204">If book updated successfully</response>
        ///<response code="400">If bookDto is null or contains invalid fields</response>
        ///<response code="401">If authorize header missing or contains invalid token</response>
        ///<response code="404">If book with id not found or author with id specified in bookDto doesn't exist</response>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(Guid id,
            [FromBody] BookForUpdateDto bookForUpdate)
        {
            var res = await _services.Books.UpdateBookAsync(id, bookForUpdate);

            if (res.Status == OpStatus.Fail)
            {
                return StatusCode((int)res.Error!.ErrorType,
                    new { message = res.Error.Description });
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
            await _services.Books.DeleteBookAsync(id);

            return NoContent();
        }
    }
}