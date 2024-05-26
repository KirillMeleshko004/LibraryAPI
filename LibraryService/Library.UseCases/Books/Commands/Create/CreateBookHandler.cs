using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Books.Commands
{
   public class CreateBookHandler : IRequestHandler<CreateBookCommand, Result<BookDto>>
   { 
      private readonly IBookRepository _books;
      private readonly IAuthorRepository _authors;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public CreateBookHandler(IBookRepository books, IAuthorRepository authors,
         IMapper mapper, ILogger logger)
      {
         _books = books;
         _authors = authors;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<BookDto>> Handle(CreateBookCommand request, 
         CancellationToken cancellationToken)
      {  
         var author = await _authors.GetAuthorByIdAsync(request.AuthorId);

         if(author == null)
         {
            _logger.LogWarning("Author with id: {Id} was not found during book creation.", 
               request.AuthorId);
               
            return Result<BookDto>
               .NotFound($"Author with id: {request.AuthorId} was not found during book creation.");
         }

         var book = _mapper.Map<Book>(request.BookDto);

         await _books.AddBookAsync(book);

         _logger.LogInformation("Book with id: {Id} was created.", book.Id);

         //Need to test, if it's needed
         var bookToReturn = _mapper.Map<BookDto>(
               await _books.GetBookByIdAsync(book.Id));

         return Result<BookDto>.Success(bookToReturn);         
      }
   }
}