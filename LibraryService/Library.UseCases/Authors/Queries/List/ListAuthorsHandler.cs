using AutoMapper;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Authors.Queries
{
   public class ListAuthorsHandler :
      IRequestHandler<ListAuthorsQuery, IEnumerable<AuthorDto>>
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

      public async Task<IEnumerable<AuthorDto>> Handle(ListAuthorsQuery request,
         CancellationToken cancellationToken)
      {
         var authors = await _repo.Authors
            .GetAuthorsAsync(request.Parameters, cancellationToken);

         var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

         return authorsToReturn;
      }
   }
}