using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class CreateBookHandler : IRequestHandler<CreateBookCommand, Result<BookDto>>
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

      public async Task<Result<BookDto>> Handle(CreateBookCommand request,
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors
            .GetAuthorByIdAsync(request.AuthorId, cancellationToken);

         if (author == null)
         {
            _logger.LogWarning(AuthorNotFoundBookCreationLog, request.AuthorId);

            return Result<BookDto>
               .InvalidData(string.Format(AuthorNotFoundBookCreation, request.AuthorId));
         }

         var book = _mapper.Map<Book>(request.BookDto);
         book.AuthorName = $"{author.FirstName} {author.LastName}";

         if (request.BookDto.Image != null && request.BookDto.ImageName != null)
         {
            book.ImagePath = await _images.SaveImageAsync(request.BookDto.Image,
               request.BookDto.ImageName);
         }

         await _repo.Books.AddBookAsync(book, cancellationToken);
         await _repo.SaveChangesAsync();

         _logger.LogInformation(BookCreatedLog, book.Id);

         var bookToReturn = _mapper.Map<BookDto>(book);

         return Result<BookDto>.Success(bookToReturn);
      }
   }
}