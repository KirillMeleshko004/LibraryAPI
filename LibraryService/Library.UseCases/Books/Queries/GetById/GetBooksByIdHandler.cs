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
   public class GetBooksByIdHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public GetBooksByIdHandler(IRepositoryManager repo, IMapper mapper, ILogger logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, 
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetBookByIdAsync(request.Id, cancellationToken);

         if(book == null)
         {
            _logger.LogWarning(BookNotFoundLog, request.Id);
            return Result<BookDto>.NotFound(string.Format(BookNotFound, request.Id));
         }

         var bookToReturn = _mapper.Map<BookDto>(book);

         return Result<BookDto>.Success(bookToReturn);
      }
   }
}