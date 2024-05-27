using AutoMapper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Queries
{
   public class GetBookByISBNHandler : IRequestHandler<GetBookByISBNQuery, Result<BookDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<GetBookByISBNHandler> _logger;

      public GetBookByISBNHandler(IRepositoryManager repo, IMapper mapper, 
         ILogger<GetBookByISBNHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<BookDto>> Handle(GetBookByISBNQuery request, 
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetBookByISBNAsync(request.ISBN, cancellationToken);

         if(book == null)
         {
            _logger.LogWarning(BookNotFoundISBNLog, request.ISBN);
            return Result<BookDto>.NotFound(string.Format(BookNotFoundISBN, request.ISBN));
         }

         var bookDto = _mapper.Map<BookDto>(book);

         return Result<BookDto>.Success(bookDto);
      }
   }
}