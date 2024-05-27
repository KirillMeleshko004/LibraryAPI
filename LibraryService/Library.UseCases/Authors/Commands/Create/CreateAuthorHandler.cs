using AutoMapper;
using Library.Domain.Entities;
using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;

namespace Library.UseCases.Authors.Commands
{
   public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, Result<AuthorDto>>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger _logger;

      public CreateAuthorHandler(IRepositoryManager repo, IMapper mapper, ILogger logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<Result<AuthorDto>> Handle(CreateAuthorCommand request, 
         CancellationToken cancellationToken)
      {
         var author = _mapper.Map<Author>(request.AuthorDto);

         await _repo.Authors.AddAuthorAsync(author, cancellationToken);
         await _repo.SaveChangesAsync();

         var authorToReturn = _mapper.Map<AuthorDto>(author);
         
         _logger.LogInformation(AuthorCreatedLog, author.Id);

         return Result<AuthorDto>.Success(authorToReturn);
      }
   }
}