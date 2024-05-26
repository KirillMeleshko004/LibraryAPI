using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Books.Queries
{
   public class GetBooksByIdHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
   {
      private readonly IBookRepository _books;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public GetBooksByIdHandler(IBookRepository books, IMapper mapper, ILogger logger)
      {
         _books = books;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, 
      CancellationToken cancellationToken)
      {
         var book = await _books.GetBookByIdAsync(request.Id);

         if(book == null)
         {
            _logger.LogWarning("Book with id: {Id} was not found.", request.Id);
            return Result<BookDto>.NotFound($"Book with id: {request.Id} was not found.");
         }

         var bookDto = _mapper.Map<BookDto>(book);

         return Result<BookDto>.Success(bookDto);
      }
   }
}