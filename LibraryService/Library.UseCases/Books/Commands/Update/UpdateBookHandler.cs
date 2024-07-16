using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IImageService _images;
      private readonly IMapper _mapper;
      private readonly ILogger<UpdateBookHandler> _logger;

      public UpdateBookHandler(IRepositoryManager repo, IImageService images,
         IMapper mapper, ILogger<UpdateBookHandler> logger)
      {
         _repo = repo;
         _images = images;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<BookDto> Handle(UpdateBookCommand request,
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetBookByIdAsync(request.BookId, cancellationToken);

         if (book == null)
         {
            _logger.LogWarning(BookNotFoundLog, request.BookId);
            throw new NotFoundException(string.Format(BookNotFound, request.BookId));
         }

         var newAuthorId = request.BookDto.AuthorId;
         var author = await _repo.Authors.GetAuthorByIdAsync(newAuthorId, cancellationToken);

         if (author == null)
         {
            _logger.LogWarning(IncorrectAuthorIdLog, newAuthorId);
            throw new UnprocessableEntityException(string.Format(IncorrectAuthorId, newAuthorId));
         }

         _mapper.Map(request.BookDto, book);
         book.AuthorName = $"{author.FirstName} {author.LastName}";
         if (request.BookDto.Image != null && request.BookDto.ImageName != null)
         {
            book.ImagePath = await _images.SaveImageAsync(request.BookDto.Image,
               request.BookDto.ImageName);
         }

         await _repo.Books.UpdateBookAsync(book, cancellationToken);
         await _repo.SaveChangesAsync();

         var bookToReturn = _mapper.Map<BookDto>(book);

         return bookToReturn;
      }
   }
}