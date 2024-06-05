using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Authors.Queries
{
   public class ListAuthorsHandler :
      IRequestHandler<ListAuthorsQuery, Result<IEnumerable<AuthorDto>>>
   {

      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<ListAuthorsHandler> _logger;

      public ListAuthorsHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<ListAuthorsHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<IEnumerable<AuthorDto>>> Handle(ListAuthorsQuery request,
         CancellationToken cancellationToken)
      {
         var authors = await _repo.Authors
            .GetAuthorsAsync(request.Parameters, cancellationToken);

         if (!authors.Any())
         {
            return Result<IEnumerable<AuthorDto>>.NotFound(AuthorsNotFound);
         }

         var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

         return Result<IEnumerable<AuthorDto>>.Success(authorsToReturn);
      }
   }
}