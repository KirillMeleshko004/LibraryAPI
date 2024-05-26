using AutoMapper;
using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Books.Commands
{
   public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Result>
   {
      private readonly IBookRepository _books;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public DeleteBookHandler(IBookRepository books, IMapper mapper, ILogger logger)
      {
         _books = books;
         _mapper = mapper;
         _logger = logger;
      }
      public async Task<Result> Handle(DeleteBookCommand request, 
         CancellationToken cancellationToken)
      {
         var book = await _books.GetBookByIdAsync(request.Id);

         //If book not exist or already deleted
         if (book == null) return Result.Success();

         await _books.DeleteBookAsync(book);

         _logger.LogInformation("Book with id: {Id} was deleted.", book.Id);

         return Result.Success();
      }
   }
}