using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Books.Queries
{
   public class ListBooksHandler :
      IRequestHandler<ListBooksQuery, Result<IEnumerable<BookDto>>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public ListBooksHandler(IRepositoryManager repo, IMapper mapper, ILogger logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<IEnumerable<BookDto>>> Handle(ListBooksQuery request, 
         CancellationToken cancellationToken)
      {
         var books = await _repo.Books.GetBooksAsync(request.Parameters, cancellationToken);

         var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);

         return Result<IEnumerable<BookDto>>.Success(booksToReturn);
      }
   }
}