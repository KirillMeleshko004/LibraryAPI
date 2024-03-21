using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Services.Books
{
    public class BooksService : IBooksService
    {

        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BookDto?> GetBookByISBDAsync(string ISBN)
        {
            throw new NotImplementedException();            
        }

        public async Task<IEnumerable<BookDto?>> GetBooksAsync(BookParameters parameters)
        {            
            throw new NotImplementedException();
        }

        public async Task<BookDto> CreateBookAsync(BookForCreationDto bookDto)
        {            
            throw new NotImplementedException();
        }

        public async Task UpdateBook(Guid id, BookForUpdateDto bookDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteBook(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}