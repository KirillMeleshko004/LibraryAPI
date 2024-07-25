using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly IImageService _images;
      private readonly ILogger<CreateBookHandler> _logger;

      public CreateBookHandler(IRepositoryManager repo, IMapper mapper,
         IImageService images, ILogger<CreateBookHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _images = images;
         _logger = logger;
      }

      public async Task<BookDto> Handle(CreateBookCommand request,
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors
            .GetSingle(a => a.Id.Equals(request.BookDto.AuthorId),
               cancellationToken: cancellationToken);

         if (author == null)
         {
            _logger.LogWarning(AuthorNotFoundBookCreationLog, request.BookDto.AuthorId);

            throw new UnprocessableEntityException(
               string.Format(AuthorNotFoundBookCreation, request.BookDto.AuthorId));
         }

         var book = _mapper.Map<Book>(request.BookDto);
         book.AuthorName = $"{author.FirstName} {author.LastName}";

         if (request.BookDto.Image != null && request.BookDto.ImageName != null)
         {
            book.ImagePath = await _images.SaveImageAsync(request.BookDto.Image,
               request.BookDto.ImageName);
         }

         _repo.Books.Create(book, cancellationToken);
         await _repo.SaveChangesAsync();

         _logger.LogInformation(BookCreatedLog, book.Id);

         book = await _repo.Books.GetSingle(b => b.Id.Equals(book.Id),
            b => b.Author!, cancellationToken);

         var bookToReturn = _mapper.Map<BookDto>(book);

         return bookToReturn;
      }
   }
}