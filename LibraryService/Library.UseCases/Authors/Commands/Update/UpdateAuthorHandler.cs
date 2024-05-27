using AutoMapper;
using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Authors.Commands
{
   public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Result<AuthorDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public UpdateAuthorHandler(IRepositoryManager repo, IMapper mapper, ILogger logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<AuthorDto>> Handle(UpdateAuthorCommand request, 
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors
            .GetAuthorByIdAsync(request.AuthorId, cancellationToken);
         
         if(author == null)
         {
            _logger.LogWarning(AuthorNotFoundLog, request.AuthorId);
            return Result<AuthorDto>
               .NotFound(string.Format(AuthorNotFound, request.AuthorId));
         }

         _mapper.Map(request.AuthorDto, author);
         await _repo.Authors.UpdateAuthorAsync(author, cancellationToken);
         await _repo.SaveChangesAsync();

         var authorToReturn = _mapper.Map<AuthorDto>(author);

         return Result<AuthorDto>.Success(authorToReturn);
      }
   }
}