using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Authors.Commands
{
   public record DeleteAuthorCommand(Guid Id) : IRequest<Result> {}
}