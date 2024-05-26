using Library.UseCases.Books.DTOs;
using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Books.Commands
{
   public record CreateBookCommand(BookForCreationDto BookDto, Guid AuthorId) 
      : IRequest<Result<BookDto>>{}
}