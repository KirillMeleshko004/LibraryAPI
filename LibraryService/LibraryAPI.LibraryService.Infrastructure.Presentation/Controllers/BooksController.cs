using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.LibraryService.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
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
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _services.Books.GetBookByIdAsync(id);

            if(book == null)
            {
                return NotFound($"Book with id: {id} not found.");
            }
            
            return Ok(book);
        } 

        [HttpGet("{ISBN}")]
        public async Task<IActionResult> GetBookByISBN(string ISBN)
        {
            var book = await _services.Books.GetBookByISBNAsync(ISBN);

            if(book == null)
            {
                return NotFound($"Book with ISBN: {ISBN} not found.");
            }
            
            return Ok(book);
        } 

        [HttpGet]
        public async Task<IActionResult> GetBooks(BookParameters parameters)
        {
            var books = await _services.Books.GetBooksAsync(parameters);
            
            return Ok(books);
        } 

        [HttpPost]
        public async Task<IActionResult> CreateBook(
            [FromBody]BookForCreationDto bookForCreation)
        {
            var createdBook = await _services.Books.CreateBookAsync(bookForCreation);
            
            return CreatedAtAction(nameof(GetBookById), 
                new { id = createdBook.Id }, 
                createdBook);
        } 

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBook(Guid id,
            [FromBody]BookForUpdateDto bookForUpdate)
        {
            var updatedBook = await _services.Books.UpdateBook(id, bookForUpdate);

            if(updatedBook == null)
            {
                return NotFound($"Book with ISBN: {id} not found.");
            }
            
            return NoContent();
        } 

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBookById(Guid id)
        {
            await _services.Books.DeleteBook(id);
            
            return NoContent();
        } 
    }
}