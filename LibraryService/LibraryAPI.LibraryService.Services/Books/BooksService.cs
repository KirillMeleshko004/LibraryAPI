using AutoMapper;
using LibraryAPI.LibraryService.Domain.Core.Entities;
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
            var book = await _repo.Books.GetBookByIdAsync(id, trackChanges : false);

            if(book == null) return null;

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;
        }

        public async Task<BookDto?> GetBookByISBNAsync(string ISBN)
        {
            var book =  await _repo.Books.GetBookByISBNAsync(ISBN, 
                trackChanges: false);

            if(book == null) return null;

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;            
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync(BookParameters parameters)
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

        public async Task<BookDto?> UpdateBook(Guid id, BookForUpdateDto bookDto)
        {
            var book = await _repo.Books.GetBookByIdAsync(id, trackChanges : true);

            if(book == null) return null;

            _mapper.Map(bookDto, book);

            await _repo.SaveChangesAsync();

            var bookToReturn = _mapper.Map<BookDto>(book);

            return bookToReturn;
        }

        public async Task DeleteBook(Guid id)
        {
            var book = await _repo.Books.GetBookByIdAsync(id, trackChanges : true);

            //If book not exist or already deleted
            if(book == null) return;

            await _repo.Books.DeleteBookAsync(book);
            
            await _repo.SaveChangesAsync();
        }

    }
}