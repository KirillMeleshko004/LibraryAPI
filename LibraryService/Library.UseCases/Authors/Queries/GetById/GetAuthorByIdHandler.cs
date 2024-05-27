using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Authors.Queries
{
   public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, Result<AuthorDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<GetAuthorByIdHandler> _logger;

      public GetAuthorByIdHandler(IRepositoryManager repo, IMapper mapper, 
         ILogger<GetAuthorByIdHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<AuthorDto>> Handle(GetAuthorByIdQuery request, 
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors.GetAuthorByIdAsync(request.Id, cancellationToken);

         if(author == null)
         {
            _logger.LogInformation(AuthorNotFoundLog, request.Id);
            return Result<AuthorDto>.NotFound(string.Format(AuthorNotFound, request.Id));
         }

         var authorToReturn = _mapper.Map<AuthorDto>(author);

         return Result<AuthorDto>.Success(authorToReturn);
      }
   }
}