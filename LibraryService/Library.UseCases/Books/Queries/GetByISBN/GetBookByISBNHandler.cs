using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Books.Queries
{
   public class GetBookByISBNHandler : IRequestHandler<GetBookByISBNQuery, Result<BookDto>>
   {
      private readonly IBookRepository _books;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public GetBookByISBNHandler(IBookRepository books, IMapper mapper, ILogger logger)
      {
         _books = books;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<BookDto>> Handle(GetBookByISBNQuery request, 
         CancellationToken cancellationToken)
      {
         var book = await _books.GetBookByISBNAsync(request.ISBN);

         if(book == null)
         {
            _logger.LogWarning("Book with ISBN: {ISBN} was not found.", request.ISBN);
            return Result<BookDto>.NotFound($"Book with ISBN: {request.ISBN} was not found.");
         }

         var bookDto = _mapper.Map<BookDto>(book);

         return Result<BookDto>.Success(bookDto);
      }
   }
}