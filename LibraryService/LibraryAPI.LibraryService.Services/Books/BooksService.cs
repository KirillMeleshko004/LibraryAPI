using AutoMapper;
using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Domain.Core.Exceptions;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Services.Books
{
    public class BooksService : IBooksService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILibraryLogger _logger;
        private readonly IMapper _mapper;

        public BooksService(IRepositoryManager respository, ILibraryLogger logger,
            IMapper mapper)
        {
            _repo = respository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            var book = await GetBookByIdAndCheckIfExistAsync(id,
                trackChanges: false);

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;
        }

        public async Task<BookDto?> GetBookByISBDAsync(string ISBN)
        {
            var book = await GetBookByISBNAndCheckIfExistAsync(ISBN,
                trackChanges: false);

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;            
        }

        public async Task<IEnumerable<BookDto?>> GetBooksAsync(BookParameters parameters)
        {            
            var books = await _repo.Books.GetBooksAsync(parameters, 
                trackChanges: false);

            var booksDTOs = _mapper.Map<IEnumerable<BookDto>>(books);

            return booksDTOs;
        }

        public async Task<BookDto> CreateBookAsync(BookForCreationDto bookDto)
        {            
            var book = _mapper.Map<Book>(bookDto);

            await _repo.Books.AddBookAsync(book);
            await _repo.SaveChangesAsync();

            var bookToReturn = _mapper.Map<BookDto>(book);

            return bookToReturn;
        }

        public async Task UpdateBook(Guid id, BookForUpdateDto bookDto)
        {
            var book = await GetBookByIdAndCheckIfExistAsync(id, trackChanges: true);

            _mapper.Map(bookDto, book);

            await _repo.SaveChangesAsync();
        }

        public async Task DeleteBook(Guid id)
        {
            var book = await GetBookByIdAndCheckIfExistAsync(id, trackChanges: false);

            await _repo.Books.DeleteBookAsync(book);
            
            await _repo.SaveChangesAsync();
        }

        private async Task<Book> GetBookByIdAndCheckIfExistAsync(Guid id, bool trackChanges)
        {
            return await _repo.Books.GetBookByIdAsync(id, trackChanges) ??
                throw new BookNotFoundException(id);
        }

        private async Task<Book> GetBookByISBNAndCheckIfExistAsync(string ISBN, 
            bool trackChanges)
        {
            return await _repo.Books.GetBookByISBNAsync(ISBN, trackChanges) ??
                throw new BookNotFoundException(ISBN);
        }

    }
}