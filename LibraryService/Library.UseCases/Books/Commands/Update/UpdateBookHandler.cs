using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Result<BookDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<UpdateBookHandler> _logger;

      public UpdateBookHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<UpdateBookHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<BookDto>> Handle(UpdateBookCommand request,
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetBookByIdAsync(request.BookId, cancellationToken);

         if (book == null)
         {
            _logger.LogWarning(BookNotFoundLog, request.BookId);
            return Result<BookDto>.NotFound(string.Format(BookNotFound, request.BookId));
         }

         var newAuthorId = request.BookDto.AuthorId;
         var author = await _repo.Authors.GetAuthorByIdAsync(newAuthorId, cancellationToken);

         if (author == null)
         {
            _logger.LogWarning(IncorrectAuthorIdLog, newAuthorId);
            return Result<BookDto>
               .InvalidData(string.Format(IncorrectAuthorId, newAuthorId));
         }

         _mapper.Map(request.BookDto, book);
         book.AuthorName = $"{author.FirstName} {author.LastName}";

         await _repo.Books.UpdateBookAsync(book, cancellationToken);
         await _repo.SaveChangesAsync();

         var bookToReturn = _mapper.Map<BookDto>(book);

         return Result<BookDto>.Success(bookToReturn);
      }
   }
}