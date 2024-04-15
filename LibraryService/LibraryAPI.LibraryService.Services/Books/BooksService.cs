using AutoMapper;
using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Domain.Core.Results;
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
            var bookRes = await GetBookByIdAsync(id, trackChanges: false);

            if (bookRes.Status == OpStatus.Fail) return null;

            var bookDto = _mapper.Map<BookDto>(bookRes.Value);

            return bookDto;
        }

        public async Task<BookDto?> GetBookByISBNAsync(string ISBN)
        {
            var bookRes = await GetBookByISBNAsync(ISBN, trackChanges: false);

            if (bookRes.Status == OpStatus.Fail) return null;

            var bookDto = _mapper.Map<BookDto>(bookRes.Value);

            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync(BookParameters parameters)
        {
            var books = await _repo.Books.GetBooksAsync(parameters,
                trackChanges: false);

            var booksDTOs = _mapper.Map<IEnumerable<BookDto>>(books);

            return booksDTOs;
        }

        public async Task<ValueOpResult<BookDto>> CreateBookAsync(BookForCreationDto bookDto)
        {
            var authorRes = await CheckAuthorExistAsync(bookDto.AuthorId);

            if (authorRes.Status == OpStatus.Fail)
                return OpResult.FailValueResult<BookDto>(authorRes);

            var book = _mapper.Map<Book>(bookDto);

            await _repo.Books.AddBookAsync(book);
            await _repo.SaveChangesAsync();

            _logger.LogInfo($"Book with id: {book.Id} was created.");

            var bookToReturn = _mapper.Map<BookDto>(
                await _repo.Books.GetBookByIdAsync(book.Id, false));

            return OpResult.SuccessValueResult(bookToReturn);
        }

        public async Task<OpResult>
            UpdateBookAsync(Guid id, BookForUpdateDto bookDto)
        {
            var authorRes = await CheckAuthorExistAsync(bookDto.AuthorId);

            if (authorRes.Status == OpStatus.Fail) return authorRes;

            var bookRes = await GetBookByIdAsync(id, trackChanges: true);

            if (bookRes.Status == OpStatus.Fail) return bookRes;

            //Since we track change of book entity, simple mapping dto into entity
            //override it fields
            _mapper.Map(bookDto, bookRes.Value);

            await _repo.SaveChangesAsync();

            return OpResult.SuccessResult();
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _repo.Books.GetBookByIdAsync(id, trackChanges: true);

            //If book not exist or already deleted
            if (book == null) return;

            await _repo.Books.DeleteBookAsync(book);

            await _repo.SaveChangesAsync();

            _logger.LogInfo($"Book with id: {book.Id} was deleted.");

        }

        #region Private methods

        private async Task<ValueOpResult<Book>> GetBookByIdAsync(Guid id, bool trackChanges)
        {
            var book = await _repo.Books.GetBookByIdAsync(id, trackChanges);

            if (book == null)
            {
                var errMes = $"Book with id: {id} was not found.";
                _logger.LogWarn(errMes);
                return OpResult.FailValueResult<Book>(errMes, ErrorType.NotFound);
            }

            return OpResult.SuccessValueResult(book);
        }

        private async Task<ValueOpResult<Book>>
            GetBookByISBNAsync(string ISBN, bool trackChanges)
        {
            var book = await _repo.Books.GetBookByISBNAsync(ISBN, trackChanges);

            if (book == null)
            {
                var errMes = $"Book with ISBN: {ISBN} was not found.";
                _logger.LogWarn(errMes);
                return OpResult.FailValueResult<Book>(errMes, ErrorType.NotFound);
            }

            return OpResult.SuccessValueResult(book);
        }

        private async Task<OpResult> CheckAuthorExistAsync(Guid id)
        {
            var author = await _repo.Authors.GetAuthorByIdAsync(id, false);

            if (author == null)
            {
                var errMes = $"Author with id: {id} was not found.";
                _logger.LogWarn(errMes);
                return OpResult.FailResult(errMes, ErrorType.NotFound);
            }

            return OpResult.SuccessResult();
        }

        #endregion

    }
}