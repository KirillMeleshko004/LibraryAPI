using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Books.Commands
{
   public record DeleteBookCommand(Guid Id) : IRequest<Result> {}
}