using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Queries
{
   public class ListBooksForReaderHandler :
      IRequestHandler<ListBooksForReaderQuery, IEnumerable<BookDto>>
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

      public async Task<IEnumerable<BookDto>> Handle(ListBooksForReaderQuery request,
         CancellationToken cancellationToken)
      {
         var reader = await _repo.Readers
            .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);

         if (reader == null)
         {
            throw new NotFoundException(ReaderNotFound);
         }

         var books = await _repo.Books
            .GetBookByReaderAsync(reader.Id, cancellationToken);

         var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);

         return booksToReturn;
      }
   }
}