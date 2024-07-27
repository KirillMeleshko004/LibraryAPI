using Library.Domain.Entities;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class BorrowBookHandler : IRequestHandler<BorrowBookCommand>
   {
      private readonly IRepositoryManager _repo;
      private readonly ILogger<BorrowBookHandler> _logger;

      public BorrowBookHandler(IRepositoryManager repo, ILogger<BorrowBookHandler> logger)
      {
         _repo = repo;
         _logger = logger;
      }

      public async Task Handle(BorrowBookCommand request, CancellationToken cancellationToken)
      {
         var book = await _repo.Books
            .GetSingle(b => b.Id.Equals(request.BookId), cancellationToken: cancellationToken);

         if (book == null)
         {
            throw new NotFoundException(string.Format(BookNotFound, request.BookId));
         }

         if (book.IsAvailable == false)
         {
            throw new UnavailableException(string.Format(BookNotAvailable, request.BookId));
         }

         var reader = await _repo.Readers
            .GetSingle(r => r.Email.Equals(request.ReaderEmail),
               cancellationToken: cancellationToken);

         if (reader == null)
         {
            reader = new Reader
            {
               Email = request.ReaderEmail
            };
            _repo.Readers.Create(reader, cancellationToken);
            await _repo.SaveChangesAsync();
         }

         book.IsAvailable = false;
         book.CurrentReaderId = reader!.Id;
         book.BorrowTime = DateTime.Now;
         book.ReturnTime = DateTime.Now.AddDays(30);

         //Test if necessary
         _repo.Books.Update(book, cancellationToken);
         await _repo.SaveChangesAsync();
      }
   }
}