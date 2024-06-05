using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Authors.Commands
{
   /// <summary>
   /// Command for author deletion. Possible response statuses: Ok.
   /// </summary>
   public record DeleteAuthorCommand(Guid Id) : IRequest<Result> { }
}