using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Queries
{
   public class ListBooksByAuthorHandler :
      IRequestHandler<ListBooksByAuthorQuery, IEnumerable<BookDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<ListBooksByAuthorHandler> _logger;

      public ListBooksByAuthorHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<ListBooksByAuthorHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }
      public async Task<IEnumerable<BookDto>> Handle(ListBooksByAuthorQuery request,
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors.GetAuthorByIdAsync(request.AuthorId,
            cancellationToken);

         if (author == null)
         {
            _logger.LogInformation(AuthorNotFoundLog, request.AuthorId);
            throw new NotFoundException(string.Format(AuthorNotFound, request.AuthorId));
         }

         var books = await _repo.Books.GetBookByAuthorAsync(
            request.Parameters,
            request.AuthorId,
            cancellationToken);

         var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);

         return booksToReturn;
      }
   }
}