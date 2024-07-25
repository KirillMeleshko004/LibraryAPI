using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Queries
{
   public class GetBookByISBNHandler : IRequestHandler<GetBookByISBNQuery, BookDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<GetBookByISBNHandler> _logger;

      public GetBookByISBNHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<GetBookByISBNHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<BookDto> Handle(GetBookByISBNQuery request,
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetSingle(b => b.ISBN.Equals(request.ISBN),
            b => b.Author!, cancellationToken);

         if (book == null)
         {
            _logger.LogWarning(BookNotFoundISBNLog, request.ISBN);
            throw new NotFoundException(string.Format(BookNotFoundISBN, request.ISBN));
         }

         var bookDto = _mapper.Map<BookDto>(book);

         return bookDto;
      }
   }
}