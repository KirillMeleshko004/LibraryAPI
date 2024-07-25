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
   public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<GetBookByIdHandler> _logger;

      public GetBookByIdHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<GetBookByIdHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<BookDto> Handle(GetBookByIdQuery request,
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetSingle(b => b.Id.Equals(request.Id),
            b => b.Author!, cancellationToken);

         if (book == null)
         {
            _logger.LogWarning(BookNotFoundLog, request.Id);
            throw new NotFoundException(string.Format(BookNotFound, request.Id));
         }

         var bookToReturn = _mapper.Map<BookDto>(book);

         return bookToReturn;
      }

   }
}