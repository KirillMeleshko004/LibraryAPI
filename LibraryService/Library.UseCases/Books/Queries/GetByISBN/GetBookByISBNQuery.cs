using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Queries
{
    public record GetBookByISBNQuery(string ISBN) : IRequest<BookDto>;

}
