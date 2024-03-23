using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Domain.Interfaces.Services
{
    public interface IBooksService
    {
        Task<BookDto?> GetBookByIdAsync(Guid id);
        Task<BookDto?> GetBookByISBNAsync(string ISBN);
        Task<IEnumerable<BookDto>> GetBooksAsync(BookParameters parameters);

        Task<BookDto> CreateBookAsync(BookForCreationDto bookDto);
        Task<BookDto?> UpdateBook(Guid id, BookForUpdateDto bookDto);
        Task DeleteBook(Guid id);
    }
}