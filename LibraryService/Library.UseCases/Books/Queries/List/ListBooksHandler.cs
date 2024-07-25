using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Books.Queries
{
   public class ListBooksHandler : IRequestHandler<ListBooksQuery, IPagedEnumerable<BookDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<ListBooksHandler> _logger;

      public ListBooksHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<ListBooksHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<IPagedEnumerable<BookDto>> Handle(ListBooksQuery request,
         CancellationToken cancellationToken)
      {
         var books = await _repo.Books.GetRange(request.Parameters,
            cancellationToken: cancellationToken);

         var booksToReturn = _mapper.Map<IPagedEnumerable<BookDto>>(books);

         return booksToReturn;
      }
   }
}