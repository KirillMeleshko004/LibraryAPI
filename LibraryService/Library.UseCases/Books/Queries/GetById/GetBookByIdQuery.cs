using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<BookDto>;

}
