using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Queries
{
   public class ListBooksForReaderHandler :
      IRequestHandler<ListBooksForReaderQuery, Result<IEnumerable<BookDto>>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<ListBooksForReaderHandler> _logger;

      public ListBooksForReaderHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<ListBooksForReaderHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<IEnumerable<BookDto>>> Handle(ListBooksForReaderQuery request,
         CancellationToken cancellationToken)
      {
         var reader = await _repo.Readers
            .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);

         if (reader == null)
         {
            return Result<IEnumerable<BookDto>>.NotFound(ReaderNotFound);
         }

         var books = await _repo.Books
            .GetBookByReaderAsync(reader.Id, cancellationToken);

         if (!books.Any())
         {
            return Result<IEnumerable<BookDto>>.NotFound(BooksNotFound);
         }

         var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);

         return Result<IEnumerable<BookDto>>.Success(booksToReturn);
      }
   }
}