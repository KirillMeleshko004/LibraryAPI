using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Commands
{
   public record CreateBookCommand(BookForCreationDto BookDto)
      : IRequest<BookDto>
   { }
}