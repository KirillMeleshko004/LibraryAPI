using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
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
         var book = await _repo.Books
            .GetSingle(b => b.Id.Equals(request.BookId), cancellationToken: cancellationToken);

         if (book == null)
         {
            throw new NotFoundException(string.Format(BookNotFound, request.BookId));
         }

         if (!request.BookDto.AuthorId.Equals(book.AuthorId))
         {
            var newAuthorId = request.BookDto.AuthorId;
            var author = await _repo.Authors.GetSingle(a => a.Id.Equals(newAuthorId),
               cancellationToken: cancellationToken);

            if (author == null)
            {
               throw new UnprocessableEntityException(string.Format(AuthorNotFound, newAuthorId));
            }

            book.AuthorName = $"{author.FirstName} {author.LastName}";
         }

         _mapper.Map(request.BookDto, book);


         if (request.BookDto.Image != null && request.BookDto.ImageName != null)
         {
            if (book.ImagePath != null)
            {
               await _images.DeleteImageAsync(book.ImagePath!);
            }

            book.ImagePath = await _images.SaveImageAsync(request.BookDto.Image,
               request.BookDto.ImageName);
         }

         _repo.Books.Update(book, cancellationToken);
         await _repo.SaveChangesAsync();

         var bookToReturn = _mapper.Map<BookDto>(book);

         return bookToReturn;
      }
   }
}