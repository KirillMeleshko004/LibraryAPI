using LibraryAPI.LibraryService.Domain.Core.Results;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;
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

        [HttpGet("isbn/{ISBN}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookByISBN(string ISBN)
        {
            var book = await _services.Books.GetBookByISBNAsync(ISBN);

            if (book == null)
            {
                return NotFound($"Book with ISBN: {ISBN} not found.");
            }

            return Ok(book);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks([FromQuery] BookParameters parameters)
        {
            var books = await _services.Books.GetBooksAsync(parameters);

            return Ok(books);
        }

        [HttpPost]
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

        [HttpPut("{id:guid}")]
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


        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteBookById(Guid id)
        {
            await _services.Books.DeleteBookAsync(id);

            return NoContent();
        }
    }
}