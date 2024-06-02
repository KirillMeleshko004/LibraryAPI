using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

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
         var books = await _repo.Books
            .GetBookByReaderAsync(request.ReaderEmail, cancellationToken);

         var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);

         return Result<IEnumerable<BookDto>>.Success(booksToReturn);
      }
   }
}