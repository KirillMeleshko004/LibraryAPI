using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class BorrowBookHandler : IRequestHandler<BorrowBookCommand, Result>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<BorrowBookHandler> _logger;

      public BorrowBookHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<BorrowBookHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result> Handle(BorrowBookCommand request,
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books
            .GetBookByIdAsync(request.BookId, cancellationToken);

         if (book == null)
         {
            _logger.LogWarning(BookNotFoundLog, request.BookId);
            return Result.NotFound(string.Format(BookNotFound, request.BookId));
         }

         if (book.IsAvailable == false)
         {
            return Result.Error("Book is not availible");
         }

         var reader = await _repo.Readers
            .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);

         if (reader == null)
         {
            await _repo.Readers.AddReaderAsync(request.ReaderEmail, cancellationToken);
            await _repo.SaveChangesAsync();
            reader = await _repo.Readers
               .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);
         }

         book.IsAvailable = false;
         book.CurrentReaderEmail = reader!.Email;
         book.BorrowTime = DateTime.Now;
         book.ReturnTime = DateTime.Now.AddDays(30);

         await _repo.SaveChangesAsync();

         return Result.Success();
      }
   }
}