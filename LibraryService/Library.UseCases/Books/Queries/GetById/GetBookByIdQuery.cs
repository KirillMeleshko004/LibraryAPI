using Library.UseCases.Books.DTOs;
using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Books.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<Result<BookDto>>;  

}
