using Library.Shared.Results;
using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Commands
{
   public record UpdateBookCommand(Guid BookId, BookForUpdateDto BookDto) : 
      IRequest<Result<BookDto>> {}
}