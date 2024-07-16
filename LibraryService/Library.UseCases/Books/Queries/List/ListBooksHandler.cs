using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Queries
{
   public class ListBooksHandler : IRequestHandler<ListBooksQuery, IEnumerable<BookDto>>
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

      public async Task<IEnumerable<BookDto>> Handle(ListBooksQuery request,
         CancellationToken cancellationToken)
      {
         var books = await _repo.Books.GetBooksAsync(request.Parameters,
            cancellationToken);

         var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);

         return booksToReturn;
      }
   }
}