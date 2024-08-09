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
      IRequestHandler<ListBooksForReaderQuery, IPagedEnumerable<BookDto>>
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

      public async Task<IPagedEnumerable<BookDto>> Handle(ListBooksForReaderQuery request,
         CancellationToken cancellationToken)
      {
         var reader = await _repo.Readers.GetSingle(r => r.Email.Equals(request.ReaderEmail),
            cancellationToken: cancellationToken);

         if (reader == null)
         {
            throw new NotFoundException(string.Format(ReaderNotFound, request.ReaderEmail));
         }

         var books = await _repo.Books.GetRange(request.Parameters,
            condition: b => b.CurrentReaderId.Equals(reader.Id),
            cancellationToken: cancellationToken);

         var booksToReturn = _mapper.Map<IPagedEnumerable<BookDto>>(books);

         return booksToReturn;
      }
   }
}