using Library.UseCases.Authors.DTOs;
using MediatR;

namespace Library.UseCases.Authors.Queries
{
   public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto>;
}